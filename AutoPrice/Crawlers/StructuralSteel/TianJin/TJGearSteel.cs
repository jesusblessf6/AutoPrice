using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.StructuralSteel.BaseMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.StructuralSteel.TianJin
{
    public class TJGearSteel : ICrawler
    {
        private const int MarketId = 489;

        //齿轮钢
        public void Run()
        {
            GearSteelBase.GetData("天津市场齿轮用钢价格行情", MarketId);
        }
    }
}
