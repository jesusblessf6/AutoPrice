﻿using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.BurdenMarket.BurdenMarketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.BurdenMarket.ImportedIronOre
{
     public class FCPort : ICrawler
    {
        private const int MarketId = 821;

        //防城港口
        public void Run()
        {
            ImportedIronOreBase.GetData("防城港口", MarketId, "防城港口铁矿石价格行情", false);
        }
    }
}
