using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.SectionSteel.SectionSteelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.SectionSteel.SectionSteelMarket
{
    public class Tangshan : ICrawler
    {
        private const int MarketId = 532;

        //型材
        public void Run()
        {
            GetDataBase.GetData("唐山", MarketId, 7, "rLeft");
        }
    }
}
