using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.StructuralSteel.BaseMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.StructuralSteel.Tangshan
{
    public class TSCarbonRound : ICrawler
    {
        private const int MarketId = 488;
        public void Run()
        {
            CarbonRoundBase.GetData("唐山市场碳结圆钢价格行情", MarketId);
        }
    }
}
