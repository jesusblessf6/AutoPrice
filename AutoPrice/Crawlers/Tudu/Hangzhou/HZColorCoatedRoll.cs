using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.Tudu.TuduBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Tudu.Hangzhou
{
     public class HZColorCoatedRoll : ICrawler
    {
        private const int MarketId = 386;
        public void Run()
        {
            ColorCoatedRollBase.GetData(MarketId, "杭州", "杭州市场彩涂板卷价格行情", 1, "b_div0");
        }
    }
}
