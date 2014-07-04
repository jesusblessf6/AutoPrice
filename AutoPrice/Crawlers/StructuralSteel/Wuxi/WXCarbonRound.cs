using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.StructuralSteel.BaseMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.StructuralSteel.Wuxi
{
    public class WXCarbonRound : ICrawler
    {
        private const int MarketId = 476;
        public void Run()
        {
            CarbonRoundBase.GetData("无锡市场碳结圆钢价格行情", MarketId);
        }
    }
}
