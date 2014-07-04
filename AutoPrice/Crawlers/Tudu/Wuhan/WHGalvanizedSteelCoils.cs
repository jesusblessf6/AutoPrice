using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.Tudu.TuduBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Tudu.Wuhan
{
     public class WHGalvanizedSteelCoils : ICrawler
    {
        private const int MarketId = 399;
        //镀锌板
        public void Run()
        {
            GalvanizedStellCoilsBase.GetData(MarketId, "武汉市场镀锌板卷价格行情", "武汉", 2, "c_div0");
        }
    }
}
