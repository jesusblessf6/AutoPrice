using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.BurdenMarket.BurdenMarketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.BurdenMarket.PigIronMarket
{
     public class Steelmaking : ICrawler
    {
         private const int MarketId = 36;

         //炼钢
         public void Run()
         {
             FBBase.SetData(MarketId, "炼钢生铁", true, 4);
         }
    }
}
