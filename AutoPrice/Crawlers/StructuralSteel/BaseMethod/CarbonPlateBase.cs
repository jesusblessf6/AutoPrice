using AutoPrice.EntityHelper;
using DataModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.StructuralSteel.BaseMethod
{
     public class CarbonPlateBase
    {
         //碳结板
        public static void GetData(string linkName, int marketId)
        {
            IWebDriver driver = new ChromeDriver();
            try
            {
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("http://www.mysteel.com/");
                var userName = driver.FindElement(By.Name("my_username"));
                userName.SendKeys("tx6215");
                var password = driver.FindElement(By.Name("my_password"));
                password.SendKeys("tx6215");
                userName.Submit();
                var steel = driver.FindElement(By.LinkText("结构钢"));
                driver.Navigate().GoToUrl(steel.GetAttribute("href"));
                Thread.Sleep(2000);

                var carbonRound = driver.FindElement(By.LinkText("碳结板"));
                driver.Navigate().GoToUrl(carbonRound.GetAttribute("href"));
                Thread.Sleep(2000);
                var date = DateTime.Now.Day + "日";
                GetPage(driver, linkName, marketId);
            }
            finally
            {
                driver.Close();
                driver.Quit();
            }
        }

        private static void GetPage(IWebDriver driver, string linkName, int marketId)
        {
            var date = DateTime.Now.Day + "日";
            var linkElements = driver.FindElements(By.PartialLinkText(linkName));
            bool isGoNext = false;
            var element = linkElements.Where(c => c.Text.Trim().Substring(0, c.Text.Trim().IndexOf("日") + 1) == date).FirstOrDefault();
            if (element != null)
            {
                driver.Navigate().GoToUrl(element.GetAttribute("href"));
                var pubDateTxt = driver.FindElement(By.ClassName("info")).Text;
                var dateTxts = pubDateTxt.Split(new[] { "　" }, StringSplitOptions.RemoveEmptyEntries);
                var pubTimeStr = dateTxts[0];
                DateTime pdate;
                if (!DateTime.TryParse(pubTimeStr, out pdate))
                {
                    pdate = DateTime.Now;
                }
                var table = driver.FindElement(By.Id("marketTable"));
                var trs = table.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"));
                for (int i = 2; i < trs.Count; i++)
                {
                    var p = new Price
                    {
                        MarketCrmId = marketId,
                        Date = pdate
                    };
                    var tds = trs[i].FindElements(By.TagName("td"));
                    var tags = new string[5];
                    for (int j = 0; j < 5; j++)
                    {
                        tags[j] = tds[j].Text;
                    }
                    string token = "|" + String.Join("|", tags) + "|";
                    p.Token = token;

                    var priceStr = tds[5].Text.Trim();//价格列
                    if (string.IsNullOrEmpty(priceStr) || priceStr == "-" || priceStr == "~")
                    {
                        p.HPirce = null;
                        p.LPrice = null;
                    }
                    else
                    {
                        if (priceStr.Contains("~"))
                        {
                            int ind = priceStr.IndexOf("~");
                            if (ind > 0)
                            {
                                var priceL = priceStr.Substring(0, ind);//最低价
                                var priceH = priceStr.Substring(ind + 1);//最高价
                                p.LPrice = decimal.Parse(priceL);
                                p.HPirce = decimal.Parse(priceH);
                            }
                        }
                        else if (priceStr.Contains("-"))
                        {
                            int ind = priceStr.IndexOf("-");
                            if (ind > 0)
                            {
                                var priceL = priceStr.Substring(0, ind);//最低价
                                var priceH = priceStr.Substring(ind + 1);//最高价
                                p.LPrice = decimal.Parse(priceL);
                                p.HPirce = decimal.Parse(priceH);
                            }
                        }
                        else
                        {
                            decimal price;
                            if (!decimal.TryParse(priceStr, out price))
                            {
                                p.HPirce = null;
                                p.LPrice = null;
                            }
                            else
                            {
                                p.LPrice = price;
                                //p.HPirce = price;
                            }
                        }
                    }

                    var tdDelta = tds[6].Text.Trim();//涨跌
                    if (!string.IsNullOrEmpty(tdDelta) && tdDelta != "-")
                    {
                        var firstTxt = tdDelta.Substring(0, 1);

                        if (firstTxt == "-")
                        {
                            p.Delta = -decimal.Parse(tdDelta.Substring(1));
                        }
                        else
                        {
                            p.Delta = decimal.Parse(tdDelta);
                        }
                    }

                    if (p.HPirce.HasValue && p.LPrice.HasValue)
                    {
                        p.Spread = (p.HPirce.Value + p.LPrice.Value) / 2;
                    }

                    var desc = tds[7].Text;//备注
                    p.Comment = desc;

                    PriceHelper.SavePrice(p);
                }
            }
            else
            {
                var beforeDate = DateTime.Now.AddDays(-1).Day + "日";
                //driver.FindElements();
                var bElement = linkElements.Where(c => c.Text.Trim().Substring(0, c.Text.Trim().IndexOf("日") + 1) == beforeDate).FirstOrDefault();
                if (bElement == null)//说明此页不包含对应此产品的前一天数据 并且在此页没有找到对应的今天的数据 所以需要翻页
                {
                    isGoNext = true;
                }
                //else说明此页包含前一天数据但是没有当天的信息 表示当天信息还没发布出来
            }
            if (isGoNext)
            {
                var nextPage = driver.FindElement(By.LinkText("下一页"));
                driver.Navigate().GoToUrl(nextPage.GetAttribute("href"));
                GetPage(driver, linkName, marketId);
            }
        }
    }
}
