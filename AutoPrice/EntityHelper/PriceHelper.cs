using System;
using System.Linq;
using DataModel;

namespace AutoPrice.EntityHelper
{
	public class PriceHelper
	{
		public static bool SavePrice(Price p)
		{
			//get the news id by price token and market_crm_id
			using (var ctx = new AutoNewsEntities())
			{
				if (ctx.Prices.Count(o => o.Token == p.Token && o.Date == p.Date && o.MarketCrmId == p.MarketCrmId) > 0)
				{
					//existed
					return false;
				}

				var n = ctx.News.Include("Market").SingleOrDefault(o => o.Name == p.Token && o.Market.MarketCrmId == p.MarketCrmId);
				if (n != null)
				{
					p.NewsId = n.Id;
				}
				else
				{
					var m = ctx.Markets.Single(o => o.MarketCrmId == p.MarketCrmId);
					var nn = new News
						{
							Name = p.Token,
							MarketId = m.Id,
							Updated = DateTime.Now
						};
					nn =ctx.News.Add(nn);
					p.NewsId = nn.Id;
				}

				ctx.Prices.Add(p);

				if (n != null && n.ProductId != null)
				{
					LogHelper.AddLog(n.Name + "已更新", n.MarketId, "success");
					n.Updated = DateTime.Now;
				}

				ctx.SaveChanges();
				return true;
			}
		}
	}
}
