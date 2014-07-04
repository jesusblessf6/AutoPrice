using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.Tudu.TuduBase;
using AutoPrice.EntityHelper;
using DataModel;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Tudu.Shanghai
{
     public class ColorCoatedRoll : ICrawler
    {
          private const int MarketId = 384;
          //彩涂板
          public void Run()
          {
              ColorCoatedRollBase.GetData(MarketId, "上海", "上海市场彩涂板卷价格行情", 1, "b_div0");
          }
    }
}
