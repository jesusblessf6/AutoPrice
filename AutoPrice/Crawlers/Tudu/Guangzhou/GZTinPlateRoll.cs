using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.Tudu.TuduBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Tudu.Guangzhou
{
     public class GZTinPlateRoll : ICrawler
    {
        private const int MarketId = 397;
        public void Run()
        {
            TinPlateRollBase.GetData(MarketId, "广州市场镀锡板价格行情", "广州", 0, "c_div1");
        }
    }
}
