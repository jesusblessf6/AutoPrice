using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.Tudu.TuduBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Tudu.Guangzhou
{
     public class GZGalvanizedSteelCoils : ICrawler
    {
         private const int MarketId = 397;
         public void Run()
         {
             GalvanizedStellCoilsBase.GetData(MarketId, "广州市场镀锌板卷价格行情", "广州", 1, "b_div2");
         }
    }
}
