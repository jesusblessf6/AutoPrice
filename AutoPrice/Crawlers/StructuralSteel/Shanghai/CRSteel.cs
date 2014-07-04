using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.StructuralSteel.BaseMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.StructuralSteel.Shanghai
{
    public class CRSteel : ICrawler
    {
        private const int MarketId = 474;

        //合结钢
        public void Run()
        {
            CRSteelBase.GetData("上海市场Cr系合结钢价格行情", MarketId);
        }
    }
}
