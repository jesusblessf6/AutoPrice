using AutoPrice.Crawlers.Base;
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

namespace AutoPrice.Crawlers.Ferroalloy.ManganeseFerroalloy
{
    public class CarbonateManganese : ICrawler
    {
        private const int MarketId = 561;
        //碳酸锰矿
        public void Run()
        {
            IWebDriver driver = new ChromeDriver();
            try
            {
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("http://www.f139.com/");

                var loginName = driver.FindElement(By.Id("userName"));
                loginName.SendKeys("f13853998584");
                var loginPassword = driver.FindElement(By.Id("passWord"));
                loginPassword.SendKeys("138539");
                var loginBtn = driver.FindElement(By.ClassName("btn_login"));
                loginBtn.Click();

                var ferroalloyUrls = driver.FindElements(By.LinkText("铁合金网"));
                driver.Navigate().GoToUrl(ferroalloyUrls[0].GetAttribute("href"));
                Thread.Sleep(2000);

                var silicon = driver.FindElement(By.LinkText("锰系"));
                driver.Navigate().GoToUrl(silicon.GetAttribute("href"));
                Thread.Sleep(2000);

                var demosticPrice = driver.FindElement(By.LinkText("国内价格"));
                driver.Navigate().GoToUrl(demosticPrice.GetAttribute("href"));
                Thread.Sleep(2000);

                var date = DateTime.Now.GetDateTimeFormats('M')[0].ToString();
                var ferrosilicons = driver.FindElements(By.LinkText(date + "国内碳酸锰矿市场价格"));
                if (ferrosilicons.Count > 0)
                {
                    driver.Navigate().GoToUrl(ferrosilicons[0].GetAttribute("href"));
                    Thread.Sleep(2000);

                    var dateSpan = driver.FindElement(By.ClassName("cGray")).Text;
                    var infoparts = dateSpan.Split(new[] { "   " }, StringSplitOptions.RemoveEmptyEntries);
                    var pubTimeStr = infoparts[0];
                    DateTime pdate;
                    if (!DateTime.TryParse(pubTimeStr, out pdate))
                    {
                        pdate = DateTime.Now;
                    }
                    var zwDiv = driver.FindElement(By.Id("zhengwen"));
                    var table = zwDiv.FindElement(By.TagName("table"));
                    var trs = table.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"));
                    var firstTds = trs[0].FindElements(By.TagName("th"));
                    var titleTxt = firstTds[4].Text.Trim();
                    var unit = titleTxt.Substring(titleTxt.IndexOf("（") + 1, titleTxt.IndexOf("）") - (titleTxt.IndexOf("（") + 1));//单位
                    List<Price> pList = new List<Price>();
                    for (int i = 1; i < trs.Count; i++)
                    {
                        var p = new Price
                        {
                            Date = pdate,
                            PriceUnit = unit,
                            MarketCrmId = MarketId
                        };

                        var tds = trs[i].FindElements(By.TagName("td"));//找到一行数据的所有列
                        var tags = new string[4];//把价格列之前的所有数据用|隔开组成一个字符串
                        for (int j = 0; j < 4; j++)
                        {
                            tags[j] = tds[j].Text;
                        }
                        string token = "|" + String.Join("|", tags) + "|";
                        p.Token = token;

                        var priceStr = tds[4].Text.Trim();//找到价格列的值
                        if (string.IsNullOrEmpty(priceStr) || priceStr == "-")
                        {
                            p.HPirce = null;
                            p.LPrice = null;
                        }
                        else
                        {
                            if (priceStr.Contains("-"))
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

                        var tdDelta = tds[5].Text.Trim();//涨跌
                        if (!string.IsNullOrEmpty(tdDelta))
                        {
                            var firstTxt = tdDelta.Substring(0, 1);
                            if (firstTxt == "涨")
                            {
                                p.Delta = decimal.Parse(tdDelta.Substring(1));
                            }
                            else if (firstTxt == "跌")
                            {
                                p.Delta = -decimal.Parse(tdDelta.Substring(1));
                            }
                            else if (firstTxt == "平")
                            {
                                p.Delta = null;
                            }
                        }
                        else
                        {
                            p.Delta = null;
                        }

                        if (p.HPirce.HasValue && p.LPrice.HasValue)
                        {
                            p.Spread = (p.HPirce.Value + p.LPrice.Value) / 2;
                        }

                        pList.Add(p);
                        if (p.Token == "|秀山|Mn18%|粉|碳酸锰矿不含税|")
                        {
                            var yn = new Price {
                                Token = "|云南|Mn18%|粉|碳酸锰矿不含税|",
                                HPirce = p.HPirce,
                                LPrice = p.LPrice,
                                Date = p.Date,
                                Delta = p.Delta,
                                Spread = p.Spread,
                                MarketCrmId = p.MarketCrmId,
                                PriceUnit = p.PriceUnit
                            };
                            pList.Add(yn);
                        }
                        //PriceHelper.SavePrice(p);
                    }

                    foreach(Price price in pList)
                    {
                        PriceHelper.SavePrice(price);
                    }
                }
            }
            finally
            {
                driver.Close();
                driver.Quit();
            }
        }
    }
}
