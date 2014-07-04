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
    public class GalvanizedSteelCoils : ICrawler
    {
        private const int MarketId = 384;
        //镀锌板
        public void Run()
        {
            GalvanizedStellCoilsBase.GetData(MarketId, "上海市场镀锌板卷价格行情", "上海", 1, "b_div0");
        }
    }
}
