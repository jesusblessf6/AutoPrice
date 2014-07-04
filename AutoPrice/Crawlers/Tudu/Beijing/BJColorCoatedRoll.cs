using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.Tudu.TuduBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Tudu.Beijing
{
     public class BJColorCoatedRoll : ICrawler
    {
        private const int MarketId = 400;
        //彩涂板
        public void Run()
        {
            ColorCoatedRollBase.GetData(MarketId, "北京", "北京市场彩涂板卷价格行情", 1, "b_div1");
        }
    }
}
