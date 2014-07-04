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

namespace AutoPrice.Crawlers.BurdenMarket.BurdenMarketBase
{
     public static class FBBase
    {
         private static List<Price> PriceList = new List<Price>();
         public static void SetData(int marketId,string linkText,bool isGoOn, int priceIndex)
         { 
            IWebDriver driver = new ChromeDriver();
            try
            {
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("http://www.f139.com/");

                var loginName = driver.FindElement(By.Id("userName"));
                loginName.SendKeys("zh15295667405");
                var loginPassword = driver.FindElement(By.Id("passWord"));
                loginPassword.SendKeys("15295667405");
                var loginBtn = driver.FindElement(By.ClassName("btn_login"));
                loginBtn.Click();

                var ferroalloyUrls = driver.FindElements(By.LinkText("废钢网"));
                driver.Navigate().GoToUrl(ferroalloyUrls[0].GetAttribute("href"));
                Thread.Sleep(2000);

                var silicon = driver.FindElement(By.LinkText("数据"));
                driver.Navigate().GoToUrl(silicon.GetAttribute("href"));
                Thread.Sleep(2000);

                var gc = driver.FindElement(By.LinkText("钢材"));
                driver.Navigate().GoToUrl(gc.GetAttribute("href"));
                Thread.Sleep(2000);

                var linkName = driver.FindElement(By.LinkText(linkText));
                driver.Navigate().GoToUrl(linkName.GetAttribute("href"));
                Thread.Sleep(2000);

                GetData(driver, marketId, isGoOn, priceIndex);

                foreach (Price price in PriceList)
                {
                    PriceHelper.SavePrice(price);
                }
            }
            finally
            {
                driver.Close();
                driver.Quit();
            }
         }
         private static void GetData(IWebDriver driver, int marketId, bool isGoOn, int priceIndex)
         {
             var dataDiv = driver.FindElement(By.Id("#"));
             var table = dataDiv.FindElement(By.TagName("div")).FindElement(By.TagName("table"));
             var trs = table.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"));

             bool isNeed = true;

             for (int i = 1; i < trs.Count - 2; i++)//最后一列分页列包含两个tr（最后一个tr里面嵌套了一个table）
             {
                 if (isNeed)
                 {
                     var dateTxt = trs[i].FindElements(By.TagName("td"))[priceIndex + 2].Text.Trim();
                     DateTime date;
                     if (!DateTime.TryParse(dateTxt, out date))
                     {
                         isNeed = false;
                         break;
                     }
                     else
                     {
                         if (date == DateTime.Parse(DateTime.Now.ToShortDateString()))
                         {
                             if(!isGoOn)
                             {
                                if(trs[i].FindElements(By.TagName("td"))[0].Text.Trim() != "氧化铁皮")
                                {
                                    isNeed = false;
                                    break;
                                }
                             }
                             var tds = trs[i].FindElements(By.TagName("td"));
                             var price = new Price
                             {
                                 MarketCrmId = marketId,
                                 Date = date
                             };
                             var tags = new string[priceIndex];
                             for (int j = 0; j < priceIndex; j++)
                             {
                                 tags[j] = tds[j].Text;
                             }
                             string token = "|" + String.Join("|", tags) + "|";
                             price.Token = token;

                             var priceStr = tds[priceIndex].Text.Trim();//价格列
                             if (string.IsNullOrEmpty(priceStr) || priceStr == "-")
                             {
                                 price.HPirce = null;
                                 price.LPrice = null;
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
                                         price.LPrice = decimal.Parse(priceL);
                                         price.HPirce = decimal.Parse(priceH);
                                     }
                                 }
                                 else
                                 {
                                     decimal p;
                                     if (!decimal.TryParse(priceStr, out p))
                                     {
                                         price.HPirce = null;
                                         price.LPrice = null;
                                     }
                                     else
                                     {
                                         price.LPrice = p;
                                         //price.HPirce = p;只有一个价格的时候 把价格给最低价
                                     }
                                 }
                             }

                             var deltaTD = tds[priceIndex + 1];
                             var deltaFont = deltaTD.FindElements(By.TagName("font"));

                             if (deltaFont.Count > 0)
                             {
                                 decimal delta;
                                 var deltaD = deltaFont[0].Text.Trim();
                                 if (!decimal.TryParse(deltaD, out delta))
                                 {
                                     price.Delta = null;
                                 }
                                 var deltaDs = deltaTD.FindElements(By.ClassName("down"));
                                 if (deltaDs.Count > 0)
                                 {
                                     price.Delta = -delta;
                                 }
                                 else
                                 {
                                     price.Delta = delta;
                                 }
                             }

                             if (price.HPirce.HasValue && price.LPrice.HasValue)
                             {
                                 price.Spread = (price.HPirce.Value + price.LPrice.Value) / 2;
                             }

                             PriceList.Add(price);
                         }
                         else
                         {
                             isNeed = false;
                             break;
                         }
                     }
                 }
             }

             if (isNeed)
             {
                 var lastTR = trs[trs.Count - 2];
                 var form = lastTR.FindElements(By.TagName("td"))[0].FindElements(By.TagName("div"))[0].FindElement(By.TagName("table")).FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"))[0].FindElements(By.TagName("td"))[0].FindElement(By.TagName("form"));
                 var link = form.FindElement(By.LinkText("下一页"));
                 driver.Navigate().GoToUrl(link.GetAttribute("href"));
                 GetData(driver, marketId, isGoOn, priceIndex);
             }
         }
    }
}
