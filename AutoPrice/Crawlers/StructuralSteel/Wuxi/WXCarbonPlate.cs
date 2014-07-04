using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.StructuralSteel.BaseMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.StructuralSteel.Wuxi
{
    public class WXCarbonPlate : ICrawler
    {
        private const int MarketId = 476;
         //碳结板
        public void Run()
        {
            CarbonPlateBase.GetData("无锡市场碳结板价格行情", MarketId);
        }
    }
}
