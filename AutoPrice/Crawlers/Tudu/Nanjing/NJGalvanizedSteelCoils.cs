using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.Tudu.TuduBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Tudu.Nanjing
{
     public class NJGalvanizedSteelCoils : ICrawler
    {
         private const int MarketId = 388;
         public void Run()
         {
             GalvanizedStellCoilsBase.GetData(MarketId, "南京市场镀锌板卷价格行情", "南京", 1, "b_div0");
         }
    }
}
