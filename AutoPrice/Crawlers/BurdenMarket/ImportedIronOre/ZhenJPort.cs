using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.BurdenMarket.BurdenMarketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.BurdenMarket.ImportedIronOre
{
     public class ZhenJPort : ICrawler
    {
        private const int MarketId = 821;

        //镇江港口
        public void Run()
        {
            ImportedIronOreBase.GetData("镇江港口", MarketId, "镇江港口铁矿石价格行情", false);
        }
    }
}
