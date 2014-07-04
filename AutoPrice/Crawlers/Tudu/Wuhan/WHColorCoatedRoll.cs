using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.Tudu.TuduBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Tudu.Wuhan
{
     public class WHColorCoatedRoll : ICrawler
    {
        private const int MarketId = 399;
        //彩涂板
        public void Run()
        {
            ColorCoatedRollBase.GetData(MarketId, "武汉", "武汉市场彩涂板卷价格行情", 2, "c_div0");
        }
    }
}
