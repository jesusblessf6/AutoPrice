using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.BurdenMarket.BurdenMarketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.BurdenMarket.IronPowderMarket
{
     public class IronPowder : ICrawler
    {
        private const int MarketId = 46;

        //铁精粉市场
         public void Run()
         {
             FBBase.SetData(MarketId, "铁精粉", true, 6);
         }
    }
}
