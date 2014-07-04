using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.Tudu.TuduBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Tudu.Beijing
{
     public class BJGalvanizedSteelCoils : ICrawler
    {
        private const int MarketId = 400;
        //镀锌板
        public void Run()
        {
            GalvanizedStellCoilsBase.GetData(MarketId, "北京市场镀锌板卷价格行情", "北京", 1, "b_div1");
        }
    }
}
