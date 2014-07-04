using System;
using System.Linq;
using DataModel;

namespace AutoPrice.EntityHelper
{
	public class NewsHelper
	{
		public static void Save(News n)
		{
			//if id = 0 then add new News; else update existed news according to the id
			using (var ctx = new AutoNewsEntities())
			{
				if (n.Id == 0)
				{
					ctx.News.Add(n);
				}
				else
				{
					var existedNews = ctx.News.Single(o => o.Id == n.Id);
					existedNews.MarketId = n.MarketId;
					existedNews.Name = n.Name;
					existedNews.ProductId = n.ProductId;
				}
				ctx.SaveChanges();
			}
		}

		public static void RegLastUpdated(int id, DateTime updated)
		{
			using (var ctx = new AutoNewsEntities())
			{
				var news = ctx.News.Single(o => o.Id == id);
				news.Updated = updated;
				ctx.SaveChanges();
			}
		}
	}
}
