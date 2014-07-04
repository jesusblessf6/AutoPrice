using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.StructuralSteel.BaseMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.StructuralSteel.JiNan
{
    public class JNCarbonRound : ICrawler
    {
        private const int MarketId = 477;
         //碳圆
        public void Run()
        {
            CarbonRoundBase.GetData("济南市场碳结圆钢价格行情", MarketId);
        }
    }
}
