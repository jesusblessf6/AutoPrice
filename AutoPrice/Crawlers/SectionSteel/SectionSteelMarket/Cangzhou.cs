using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.SectionSteel.SectionSteelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.SectionSteel.SectionSteelMarket
{
    public class Cangzhou : ICrawler
    {
        private const int MarketId = 535;

        //型材
        public void Run()
        {
            GetDataBase.GetData("沧州市场角钢价格行情", MarketId, 7, "rLeft");
        }
    }
}
