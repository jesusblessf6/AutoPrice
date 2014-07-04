using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.SectionSteel.SectionSteelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.SectionSteel.SectionSteelMarket
{
    public class Kunming : ICrawler
    {
        private const int MarketId = 571;

        //型材
        public void Run()
        {
            GetDataBase.GetData("昆明市场工角槽钢价格行情", MarketId, 9, "rRihgt");
        }
    }
}
