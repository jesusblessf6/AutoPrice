using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.StructuralSteel.BaseMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.StructuralSteel.Shanghai
{
     public class GearSteel : ICrawler
    {
        private const int MarketId = 474;

        //齿轮
        public void Run()
        {
            GearSteelBase.GetData("上海市场齿轮用钢价格行情", MarketId);
        }
    }
}
