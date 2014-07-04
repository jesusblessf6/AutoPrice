using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.StructuralSteel.BaseMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.StructuralSteel.TianJin
{
    public class TJCarbonRound : ICrawler
    {
        private const int MarketId = 489;
        public void Run()
        {
            CarbonRoundBase.GetData("天津市场碳结圆钢价格行情", MarketId);
        }
    }
}
