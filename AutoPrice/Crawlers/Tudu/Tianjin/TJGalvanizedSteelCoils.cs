using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.Tudu.TuduBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Tudu.Tianjin
{
     public class TJGalvanizedSteelCoils : ICrawler
    {
        private const int MarketId = 401;
        //镀锌板
        public void Run()
        {
            GalvanizedStellCoilsBase.GetData(MarketId, "天津市场镀锌板卷价格行情", "天津", 1, "b_div1");
        }
    }
}
