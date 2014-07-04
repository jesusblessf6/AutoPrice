using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.Tudu.TuduBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Tudu.Ningbo
{
     public class NBGalvanizedSteelCoils : ICrawler
    {
         private const int MarketId = 394;
         public void Run()
         {
             GalvanizedStellCoilsBase.GetData(MarketId, "宁波市场镀锌板卷价格行情", "宁波", 1, "b_div0");
         }
    }
}
