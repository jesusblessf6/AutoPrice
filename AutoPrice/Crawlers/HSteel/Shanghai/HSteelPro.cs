﻿using AutoPrice.Crawlers.Base;
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

namespace AutoPrice.Crawlers.HSteel.Shanghai
{
     public class HSteelPro : ICrawler
    {
         private const int MarketId = 516;

         public void Run()
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

                 var hsteel = driver.FindElement(By.LinkText("H型钢"));
                 driver.Navigate().GoToUrl(hsteel.GetAttribute("href"));
                 Thread.Sleep(2000);

                 var divMore = driver.FindElements(By.ClassName("mRow"))[2].FindElement(By.ClassName("rLeft")).FindElement(By.ClassName("more")).FindElement(By.LinkText("更多>>"));
                 driver.Navigate().GoToUrl(divMore.GetAttribute("href"));
                 Thread.Sleep(2000);

                 var date = DateTime.Now.Day + "日";
                 var hSHSteel = driver.FindElement(By.LinkText(date + "上海市场H型钢价格行情"));
                 if(hSHSteel != null)
                 {
                     driver.Navigate().GoToUrl(hSHSteel.GetAttribute("href"));
                     Thread.Sleep(2000);

                     var timeSpan = driver.FindElement(By.ClassName("info")).Text;
                     var timeSplit = timeSpan.Split(new[] { "　" }, StringSplitOptions.RemoveEmptyEntries);
                     var pubTime = timeSplit[0];
                     DateTime pdate;
                     if (!DateTime.TryParse(pubTime, out pdate))
                     {
                         pdate = DateTime.Now;
                     }

                     var table = driver.FindElement(By.Id("text22222")).FindElement(By.TagName("table"));
                     var trs = table.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"));
                     var firstTds = trs[0].FindElements(By.TagName("td"));
                     var titleTxt = firstTds[4].Text.Trim();
                     var unit = titleTxt.Substring(titleTxt.IndexOf("（") + 1, titleTxt.IndexOf("）") - (titleTxt.IndexOf("（") + 1));//单位
                     for (int i = 2; i < trs.Count; i++)
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

                         var desc = tds[6].Text;//备注
                         p.Comment = desc;
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