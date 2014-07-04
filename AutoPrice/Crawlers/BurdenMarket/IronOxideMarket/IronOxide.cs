using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.BurdenMarket.BurdenMarketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.BurdenMarket.IronOxideMarket
{
     public class IronOxide : ICrawler
    {
        private const int MarketId = 811;

        //氧化铁皮
         public void Run()
         {
             FBBase.SetData(MarketId, "废钢", false, 5);
         }
    }
}
