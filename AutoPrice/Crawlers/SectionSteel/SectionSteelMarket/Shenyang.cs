using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.SectionSteel.SectionSteelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.SectionSteel.SectionSteelMarket
{
    public class Shenyang : ICrawler
    {
        private const int MarketId = 557;

        //型材
        public void Run()
        {
            GetDataBase.GetData("沈阳市场工角槽钢价格行情", MarketId, 8, "rRihgt");
        }
    }
}
