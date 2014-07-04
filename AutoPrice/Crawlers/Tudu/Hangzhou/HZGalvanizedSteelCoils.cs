using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.Tudu.TuduBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Tudu.Hangzhou
{
     public class HZGalvanizedSteelCoils : ICrawler
    {
         private const int MarketId = 386;
         public void Run()
         {
             GalvanizedStellCoilsBase.GetData(MarketId, "杭州市场镀锌板卷价格行情", "杭州", 1, "b_div0");
         }
    }
}
