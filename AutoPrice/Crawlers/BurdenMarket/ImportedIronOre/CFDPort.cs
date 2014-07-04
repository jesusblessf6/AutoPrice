using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.BurdenMarket.BurdenMarketBase;
using AutoPrice.EntityHelper;
using DataModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.BurdenMarket.ImportedIronOre
{
     public class CFDPort : ICrawler
    {
        //曹妃甸港口
        private const int MarketId = 821;

         public void Run()
         {
             ImportedIronOreBase.GetData("曹妃甸港口", MarketId, "曹妃甸港口铁矿石价格行情", true);
         }
    }
}
