using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoPrice.EntityHelper;
using DataModel;

namespace AutoPrice.Crawlers.SectionSteel.SectionSteelBase
{
     public class GetDataBase
    {
        public static int i = 0;
         public static void GetData(string linkName, int marketId, int rowIndex, string className)
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
                var steel = driver.FindElement(By.LinkText("型钢"));
                driver.Navigate().GoToUrl(steel.GetAttribute("href"));
                Thread.Sleep(2000);

                var mRows = driver.FindElement(By.ClassName("midCol")).FindElements(By.ClassName("mRow"));
                if(mRows != null && mRows.Count > 0)
                {
                    var mRow = mRows[rowIndex];
                    var moreLink = mRow.FindElement(By.ClassName(className)).FindElement(By.ClassName("more")).FindElement(By.TagName("a"));
                    driver.Navigate().GoToUrl(moreLink.GetAttribute("href"));
                    Thread.Sleep(2000);
                     var date = DateTime.Now.Day + "日";
                    
                    if(linkName == "上海" || linkName == "唐山")
                    {
                        var dataLinks = driver.FindElements(By.PartialLinkText(date + linkName));
                        var linkElements = dataLinks.Where(c => c.Text.Trim().Contains("价格行情")).ToList();
                        if(linkElements != null && linkElements.Count > 0)
                        {
                            GetDataL(driver, linkElements[i], marketId);
                            i++;
                            if (i < linkElements.Count)
                            {
                                GetData(linkName, marketId, rowIndex, className);
                            }
                        }
                        //if(linkElements != null && linkElements.Count > 0)
                        //{
                        //    foreach(var link in linkElements)
                        //    {
                        //        GetDataL(driver, link, marketId);
                        //    }
                        //}
                    }
                    else
                    {
                        var dataLink = driver.FindElement(By.LinkText(date + linkName));
                        if (dataLink != null)
                        {
                            GetDataL(driver, dataLink, marketId);
                        }
                    }
                }
                Thread.Sleep(2000);
                
            }
            finally
            {
                driver.Close();
                driver.Quit();
            }
        }

         private static void GetDataL(IWebDriver driver,IWebElement element, int marketId)
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
                 var tags = new string[4];
                 for (int j = 0; j < 4; j++)
                 {
                     tags[j] = tds[j].Text;
                 }
                 string token = "|" + String.Join("|", tags) + "|";
                 p.Token = token;

                 var priceStr = tds[4].Text.Trim();//价格列
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

                 var tdDelta = tds[5].Text.Trim();//涨跌
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

                 var desc = tds[6].Text;//备注
                 p.Comment = desc;

                 PriceHelper.SavePrice(p);

             }
         }
    }
}
