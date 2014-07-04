using AutoPrice.Crawlers.Base;
using AutoPrice.EntityHelper;
using DataModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Ferroalloy.FerroalloyCoke
{
     public class Coke : ICrawler
    {
         private const int MarketId = 664;
         public List<Price> PriceList = new List<Price>();

         //冶金焦、铸造焦
         public void Run()
         { 
            IWebDriver driver = new ChromeDriver();
            try
            {
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl("http://data.f139.com/list.do?pid=3&vid=126");

                var loginNames = driver.FindElements(By.Id("userName"));
                if(loginNames.Count > 0)
                {
                    var loginName = loginNames[0];
                    loginName.SendKeys("zh15295667405");
                    var loginPassword = driver.FindElement(By.Id("passWord"));
                    loginPassword.SendKeys("15295667405");
                    var loginBtn = driver.FindElement(By.Id("submitbtn"));
                    loginBtn.Click();
                }
                var divP = driver.FindElement(By.Id("thjprolist"));
                var divs = divP.FindElements(By.TagName("div"));
                var yCokeDiv = divs[8].FindElement(By.LinkText("冶金焦"));//冶金焦链接
                //var zCokeDiv = divs[8].FindElement(By.LinkText("铸造焦"));//铸造焦链接
                driver.Navigate().GoToUrl(yCokeDiv.GetAttribute("href"));
                GetData(driver);
                var divPz = driver.FindElement(By.Id("thjprolist"));
                var divSz = divPz.FindElements(By.TagName("div"));
                var zCokeDiv = divSz[8].FindElement(By.LinkText("铸造焦"));//铸造焦链接
                driver.Navigate().GoToUrl(zCokeDiv.GetAttribute("href"));
                GetData(driver);

                foreach(Price price in PriceList)
                {
                    PriceHelper.SavePrice(price);
                }
            }
            finally {
                driver.Close();
                driver.Quit();
            }
         }

         private void GetData(IWebDriver driver)
         {
             //var rightDiv = driver.FindElement(By.ClassName("width694_right2"));
             //var divs = rightDiv.FindElements(By.TagName("div"));
             //var dataDiv = divs[3];//找到包含数据表的DIV
             var dataDiv = driver.FindElement(By.Id("#"));
             var table = dataDiv.FindElement(By.TagName("div")).FindElement(By.TagName("table"));
             var trs = table.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"));

             bool isNeed = true;
            
             for (int i = 1; i < trs.Count - 2;i++ )//最后一列分页列包含两个tr（最后一个tr里面嵌套了一个table）
             {
                 if(isNeed)
                 {
                     var dateTxt = trs[i].FindElements(By.TagName("td"))[7].Text.Trim();
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
                             var tds = trs[i].FindElements(By.TagName("td"));
                             var price = new Price
                             {
                                 MarketCrmId = MarketId,
                                 Date = date
                             };
                             var tags = new string[5];
                             for (int j = 0; j < 5;j++ )
                             {
                                 tags[j] = tds[j].Text;
                             }
                             string token = "|" + String.Join("|", tags) + "|";
                             price.Token = token;

                             var priceStr = tds[5].Text.Trim();//价格列
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
                                         //price.HPirce = p; 只有一个价格的时候 把价格给最低价
                                     }
                                 }
                             }

                             var deltaTD = tds[6];
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

             if(isNeed)
             {
                 var lastTR = trs[trs.Count - 2];
                 var form = lastTR.FindElements(By.TagName("td"))[0].FindElements(By.TagName("div"))[0].FindElement(By.TagName("table")).FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"))[0].FindElements(By.TagName("td"))[0].FindElement(By.TagName("form"));
                 var link = form.FindElement(By.LinkText("下一页"));
                 driver.Navigate().GoToUrl(link.GetAttribute("href"));
                 GetData(driver);
             }
         }
    }
}
