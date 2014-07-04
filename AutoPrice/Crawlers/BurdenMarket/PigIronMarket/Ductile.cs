using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.BurdenMarket.BurdenMarketBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.BurdenMarket.PigIronMarket
{
     public class Ductile : ICrawler
    {
        private const int MarketId = 36;

        //球墨铸铁
         public void Run()
         {
             FBBase.SetData(MarketId, "球墨铸铁", true, 5);
         }
    }
}
