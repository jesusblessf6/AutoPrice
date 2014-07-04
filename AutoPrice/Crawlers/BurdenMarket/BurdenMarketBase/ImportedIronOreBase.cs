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
     public static class ImportedIronOreBase
    {
         public static void GetData(string portName,int marketId, string linkText, bool isMany)
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

                 var ll = driver.FindElement(By.LinkText("炉料"));
                 driver.Navigate().GoToUrl(ll.GetAttribute("href"));
                 Thread.Sleep(2000);

                 var jkg = driver.FindElement(By.LinkText("进口矿"));
                 driver.Navigate().GoToUrl(jkg.GetAttribute("href"));
                 Thread.Sleep(2000);

                 var gkhq = driver.FindElement(By.LinkText("港口行情"));
                 driver.Navigate().GoToUrl(gkhq.GetAttribute("href"));
                 Thread.Sleep(2000);

                 var date = DateTime.Now.Day + "日";
                 var gk = driver.FindElement(By.LinkText(date + linkText));
                 if (gk != null)
                 {
                     driver.Navigate().GoToUrl(gk.GetAttribute("href"));
                     Thread.Sleep(2000);

                     var timeSpan = driver.FindElement(By.ClassName("info")).Text;
                     var timeSplit = timeSpan.Split(new[] { "　" }, StringSplitOptions.RemoveEmptyEntries);
                     var pubTime = timeSplit[0];
                     DateTime pdate;
                     if (!DateTime.TryParse(pubTime, out pdate))
                     {
                         pdate = DateTime.Now;
                     }

                     var textDiv = driver.FindElement(By.Id("text"));
                     var tables = textDiv.FindElements(By.TagName("table"));
                     var divs = textDiv.FindElements(By.TagName("div"));
                     int index = 0;
                     if (isMany)
                     {
                         index = 2;
                     }
                     var table = tables[index];
                     var trs = table.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"));
                     var firstTds = trs[0].FindElements(By.TagName("td"));
                     var titleTxt = firstTds[3].Text.Trim();
                     var unit = titleTxt.Substring(titleTxt.IndexOf("（") + 1, titleTxt.IndexOf("）") - (titleTxt.IndexOf("（") + 1));//单位

                     for (int i = 1; i < trs.Count; i++)
                     {
                         var p = new Price
                         {
                             Date = pdate,
                             PriceUnit = unit,
                             MarketCrmId = marketId
                         };

                         var tds = trs[i].FindElements(By.TagName("td"));//找到一行数据的所有列
                         var tags = new string[3];//把价格列之前的所有数据用|隔开组成一个字符串
                         for (int j = 0; j < 3; j++)
                         {
                             tags[j] = tds[j].Text;
                         }
                         string token = "|" + portName + "|" + String.Join("|", tags) + "|";
                         p.Token = token;

                         var priceStr = tds[3].Text.Trim();//找到价格列的值
                         if (priceStr.Contains("("))
                         {
                             priceStr = priceStr.Substring(0, priceStr.IndexOf("("));
                         }
                         if (priceStr.Contains("（"))
                         {
                             priceStr = priceStr.Substring(0, priceStr.IndexOf("（"));
                         }
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
                                     //p.HPirce = price; 只有一个价格的时候 把价格给最低价
                                 }
                             }
                         }

                         var tdDelta = tds[4].Text.Trim();//涨跌
                         if (!string.IsNullOrEmpty(tdDelta) && tdDelta != "-")
                         {
                             var firstTxt = tdDelta.Substring(0, 1);
                             if (firstTxt == "+")
                             {
                                 p.Delta = decimal.Parse(tdDelta.Substring(1));
                             }
                             else if (firstTxt == "-")
                             {
                                 p.Delta = -decimal.Parse(tdDelta.Substring(1));
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

                         PriceHelper.SavePrice(p);
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
