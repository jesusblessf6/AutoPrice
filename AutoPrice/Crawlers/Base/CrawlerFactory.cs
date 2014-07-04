using System;
using System.Collections.Generic;
using System.Linq;
using DataModel;

namespace AutoPrice.Crawlers.Base
{
	public class CrawlerFactory
	{
		private static CrawlerFactory _cf;

		protected CrawlerFactory()
		{
		}

		public static CrawlerFactory GetInstance()
		{
			return _cf ?? (_cf = new CrawlerFactory());
		}

		public ICrawler GetCrawler(string name)
		{
			Type t = Type.GetType(name);
			if (t != null)
			{
				return (ICrawler) Activator.CreateInstance(t, null);
			}

			return null;
		}

		public List<ICrawler> GetCrawlers(int marketId)
		{
			var result = new List<ICrawler>();
			using (var ctx = new AutoNewsEntities())
			{
				var crawlers = ctx.Crawlers.Where(o => o.MarketId == marketId).ToList();
				foreach (var crawler in crawlers)
				{
					Type t = Type.GetType(crawler.Name);
					if (t != null)
					{
						result.Add((ICrawler) Activator.CreateInstance(t, null));
					}
				}

				return result;
			}
		}
	}
}