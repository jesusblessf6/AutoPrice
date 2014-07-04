using AutoPrice.Crawlers.Base;
using AutoPrice.EntityHelper;
using DataModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace AutoPrice.Crawlers.MetalSpot
{
    public class Chinalco : ICrawler
    {
	    private const int MarketId = 33;

	    public void Run()
        {
            IWebDriver driver = new ChromeDriver();
			try
			{
				driver.Manage().Window.Maximize();

				driver.Navigate().GoToUrl("http://www.chalco.com.cn/zl/web/chalco_more.jsp?ColumnID=69"); //中铝

				var tables = driver.FindElements(By.ClassName("font01"));
				var table1 = tables[0]; //主要取日期和单位
				var trs1 = table1.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"));
				string date = trs1[1].FindElement(By.TagName("td")).Text.Trim();
				date = date.Substring(date.IndexOf("：") + 1);
				DateTime pdt;
				if (!DateTime.TryParse(date, out pdt))
				{
					pdt = DateTime.Now;
				}
				string unit1 = trs1[4].FindElements(By.TagName("td"))[1].Text.Trim(); //单位
				unit1 = unit1.Substring(unit1.IndexOf("：") + 1);
				var table2 = tables[2]; //华东市场|华南市场|西南市场
				var trs2 = table2.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"));
				string product1 = trs2[2].FindElements(By.TagName("td"))[0].Text.Trim(); //产品名字
				var tdsArea = trs2[1].FindElements(By.TagName("td"));
				var tdsPrice = trs2[2].FindElements(By.TagName("td"));
				for (int i = 1; i < 4; i++)
				{
					var newPrice = new Price
						               {
							               MarketCrmId = MarketId,
							               Date = pdt
						               };
					string token = "|" + product1 + "|" + tdsArea[i].Text.Trim() + "|";
					newPrice.Token = token;
					var price = tdsPrice[3 + i].Text.Trim();
					decimal p;
					if (!string.IsNullOrEmpty(price) && decimal.TryParse(price, out p))
					{
						newPrice.HPirce = p;
						newPrice.LPrice = p;
						newPrice.Spread = p;
					}
					newPrice.PriceUnit = unit1;
					
					PriceHelper.SavePrice(newPrice);
				}

				var newPrice2 = new Price
					                {
						                MarketCrmId = MarketId,
						                Date = pdt
					                };
				var table3 = tables[3]; //取氧化铝单位
				string unit2 =
					table3.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"))[0].FindElements(By.TagName("td"))[1].Text
					                                                                                                           .Trim();
				unit2 = unit2.Substring(unit2.IndexOf("：") + 1);
				var table4 = tables[4]; //取氧化铝价格
				var trs4 = table4.FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"));
				string product2 = trs4[2].FindElements(By.TagName("td"))[0].Text.Trim(); //产品名字
				string token2 = "|" + product2 + "|";
				var price2 = trs4[2].FindElements(By.TagName("td"))[3].Text.Trim();
				decimal p2;
				if (!string.IsNullOrEmpty(price2) && decimal.TryParse(price2, out p2))
				{
					newPrice2.HPirce = p2;
					newPrice2.LPrice = p2;
					newPrice2.Spread = p2;
				}
				newPrice2.PriceUnit = unit2;
				newPrice2.Token = token2;
				
				PriceHelper.SavePrice(newPrice2);
			}
			finally
			{
				driver.Close();
				driver.Quit();
			}
        }
    }
}
