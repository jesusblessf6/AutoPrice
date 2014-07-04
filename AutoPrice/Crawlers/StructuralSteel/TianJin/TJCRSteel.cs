using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.StructuralSteel.BaseMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.StructuralSteel.TianJin
{
    public class TJCRSteel : ICrawler
    {
        private const int MarketId = 489;

        //合结钢
        public void Run()
        {
            CRSteelBase.GetData("天津市场Cr系合结钢价格行情", MarketId);
        }
    }
}
