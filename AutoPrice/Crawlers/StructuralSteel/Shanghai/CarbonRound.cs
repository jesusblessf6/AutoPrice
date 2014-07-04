using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.StructuralSteel.BaseMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.StructuralSteel.Shanghai
{
     public class CarbonRound : ICrawler
    {
         private const int MarketId = 474;
         public void Run()
         {
             CarbonRoundBase.GetData("上海市场碳结圆钢价格行情", MarketId);
         }
    }
}
