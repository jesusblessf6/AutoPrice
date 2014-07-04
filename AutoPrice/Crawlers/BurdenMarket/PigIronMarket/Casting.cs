using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.BurdenMarket.BurdenMarketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.BurdenMarket.PigIronMarket
{
     public class Casting : ICrawler
    {
         private const int MarketId = 36;

         //铸造铁
         public void Run()
         {
             FBBase.SetData(MarketId, "铸造生铁", true, 4);
         }
    }
}
