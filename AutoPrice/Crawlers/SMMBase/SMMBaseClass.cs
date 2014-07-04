using AutoPrice.EntityHelper;
using DataModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Text.RegularExpressions;

namespace AutoPrice.Crawlers.SMMBase
{
    public class SMMBaseClass
    {
        public static void ResolvePage(int marketId,string url)
        {
            IWebDriver driver = new ChromeDriver();
            try
            {
                driver.Manage().Window.Maximize();

                driver.Navigate().GoToUrl(url);
                var table = driver.FindElement(By.ClassName("tab_s1"));
                var tBody = table.FindElement(By.TagName("tbody"));
                var lines = tBody.FindElements(By.TagName("tr"));//找到数据表的所有行

                for (int i = 1; i < lines.Count; i++)
                {
                    var p = new Price
                    {
                        MarketCrmId = marketId
                    };
                    var tds = lines[i].FindElements(By.TagName("td"));//找到一行数据的所有列
                    var tags = new string[3];//把价格列之前的所有数据用|隔开组成一个字符串
                    for (int j = 0; j < 3; j++)
                    {
                        tags[j] = tds[j].Text;
                    }
                    string token = "|" + String.Join("|", tags) + "|";
                    p.Token = token;

                    var priceStr = tds[3].Text.Trim();//找到价格列的值
                    if (!string.IsNullOrEmpty(priceStr) && priceStr != "-")
                    {
                        int index = -1;
                        for (int q = 0; q < priceStr.Length; q++)
                        {
                            if (Regex.IsMatch(priceStr[q].ToString(), @"^[\u4e00-\u9fa5]+$"))
                            {
                                index = q;
                                break;
                            }
                        }
                        if (index > 0)//表示包含汉字并且汉字的起始位置不是第一位
                        {
                            var priceS = priceStr.Substring(0, index);//截取汉字之前的字符串
                            if (priceS.Contains("-"))
                            {
                                int ind = priceS.IndexOf("-");
                                if (ind > 0)
                                {
                                    var priceL = priceS.Substring(0, ind);//最低价
                                    var priceH = priceS.Substring(ind + 1);//最高价
                                    p.LPrice = decimal.Parse(priceL);
                                    p.HPirce = decimal.Parse(priceH);
                                }
                            }
                            else
                            {
                                p.LPrice = decimal.Parse(priceS);
                                //p.HPirce = decimal.Parse(priceS);
                            }
                        }
                        if (index != -1)
                        {
                            p.PriceUnit = priceStr.Substring(index);
                        }
                    }
                    var avgPrice = tds[4].Text.Trim();//均价
                    decimal avg;
                    if (!string.IsNullOrEmpty(avgPrice) && decimal.TryParse(avgPrice, out avg))
                    {
                        p.Spread = avg;
                    }
                    var delta = tds[5].Text.Trim();//涨跌
                    decimal deltaResult;
                    if (!string.IsNullOrEmpty(delta) && decimal.TryParse(delta, out deltaResult))
                    {
                        p.Delta = deltaResult;
                    }
                    var date = tds[6].Text.Trim();//日期
                    DateTime pdt;
                    if (!DateTime.TryParse(date, out pdt))
                    {
                        pdt = DateTime.Now;
                    }
                    p.Date = pdt;
                    
                    PriceHelper.SavePrice(p);
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
