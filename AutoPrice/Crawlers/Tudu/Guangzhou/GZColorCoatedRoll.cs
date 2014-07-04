using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.Tudu.TuduBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Tudu.Guangzhou
{
     public class GZColorCoatedRoll : ICrawler
    {
         private const int MarketId = 397;
         public void Run()
         {
             ColorCoatedRollBase.GetData(MarketId, "广州", "广州市场彩涂板卷价格行情", 1, "b_div2");
         }
    }
}
