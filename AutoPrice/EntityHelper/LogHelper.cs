using System;
using System.Linq;
using DataModel;

namespace AutoPrice.EntityHelper
{
	public class LogHelper
	{
		public static void AddLog(string message, int marketId, string type)
		{
			using (var ctx = new AutoNewsEntities())
			{
				var market = ctx.Markets.Include("MarketType").Single(o => o.Id == marketId);
				ctx.Logs.Add(new Log
					             {
						             Message = market.MarketType.Name + "=>" + market.Name +"=>"+message,
						             MarketId = marketId,
						             Type = type,
						             LogTime = DateTime.Now
					             });

				ctx.SaveChanges();
			}
		}
	}
}
