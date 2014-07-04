using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.StructuralSteel.BaseMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.StructuralSteel.Hangzhou
{
     public class HZCarbonRound : ICrawler
    {
        private const int MarketId = 475;
        public void Run()
        {
            CarbonRoundBase.GetData("杭州市场碳结圆钢价格行情", MarketId);
        }
    }
}
