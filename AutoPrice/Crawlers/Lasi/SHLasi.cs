using System;
using System.Globalization;
using System.Threading;
using AutoPrice.Crawlers.Base;
using AutoPrice.EntityHelper;
using DataModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AutoPrice.Crawlers.Lasi
{
	public class SHLasi : ICrawler
	{
		private const int MarketId = 693;

		public void Run()
		{
			IWebDriver driver = new ChromeDriver();
			try
			{
				
				driver.Manage().Window.Maximize();

				driver.Navigate().GoToUrl("http://list1.mysteel.com/market/p-405-----010211-0--------1.html");
				var loginNames = driver.FindElements(By.Name("my_username"));
				if (loginNames.Count > 0)
				{
					loginNames[0].SendKeys("tx6215");
					//not logined
					var loginPwds = driver.FindElements(By.Name("my_password"));
					loginPwds[0].SendKeys("tx6215");

					loginNames[0].Submit();
				}

				var d = DateTime.Today.Day;
				var ts = driver.FindElements(By.LinkText(d.ToString(CultureInfo.InvariantCulture) + "日上海市场拉丝材价格行情"));
				driver.Navigate().GoToUrl(ts[0].GetAttribute("href"));
				Thread.Sleep(2000);


				//get publish time
				var infos = driver.FindElement(By.ClassName("info")).Text;
				var infoparts = infos.Split(new[] {"　"}, StringSplitOptions.RemoveEmptyEntries);
				var pubTimeStr = infoparts[0];

				DateTime pdt;
				if (!DateTime.TryParse(pubTimeStr, out pdt))
				{
					pdt = DateTime.Now;
				}

				var priceLines =
					driver.FindElement(By.Id("marketTable")).FindElement(By.TagName("tbody")).FindElements(By.TagName("tr"));

				for (int i = 1; i < priceLines.Count; i++)
				{
					var p = new Price
						        {
							        Date = pdt,
							        MarketCrmId = MarketId
						        };

					var fields = priceLines[i].FindElements(By.TagName("td"));
					var tags = new string[5];
					for (int j = 0; j < 5; j++)
					{
						tags[j] = fields[j].Text;
					}
					string token = "|" + String.Join("|", tags) + "|";
					p.Token = token;

					var priceStr = fields[5].Text.Trim();
					decimal priceD;
					if (string.IsNullOrWhiteSpace(priceStr) || priceStr == "-" || !decimal.TryParse(priceStr, out priceD))
					{
						p.LPrice = null;
					}
					else
					{
						p.LPrice = priceD;
					}

					var deltaStr = fields[6].Text.Trim();
					decimal deltaD;
					if (string.IsNullOrWhiteSpace(deltaStr) || priceStr == "-" || !decimal.TryParse(deltaStr, out deltaD))
					{
						p.Delta = null;
					}
					else
					{
						p.Delta = deltaD;
					}

					p.Comment = fields[7].Text.Trim();
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