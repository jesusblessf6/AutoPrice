using System;
using System.Linq;
using DataModel;

namespace AutoPrice.EntityHelper
{
	public class MarketHelper
	{
		public static int Save(Market m)
		{
			var ctx = new AutoNewsEntities();
			if (m.Id == 0)
			{
				if (ctx.Markets.Count(o => (o.Name == m.Name && o.MarketTypeId == m.MarketTypeId) || o.MarketCrmId == m.MarketCrmId) > 0)
				{
					throw new Exception("名称或CRM ID已存在！");
				}
				ctx.Markets.Add(m);
				return 1;
			}
			
			//id != 0
            if (ctx.Markets.Count(o => ((o.Name == m.Name && o.MarketTypeId == m.MarketTypeId) || o.MarketCrmId == m.MarketCrmId) && o.Id != m.Id) > 0)
			{
				throw new Exception("名称或CRM ID已存在！");
			}
			var market = ctx.Markets.Single(o => o.Id == m.Id);
			market.Auto = m.Auto;
			market.EndTime = m.EndTime;
			market.MarketCrmId = m.MarketCrmId;
			market.Name = m.Name;
			market.StartTime = m.StartTime;
			ctx.SaveChanges();
			return 2;
		}
	}
}
