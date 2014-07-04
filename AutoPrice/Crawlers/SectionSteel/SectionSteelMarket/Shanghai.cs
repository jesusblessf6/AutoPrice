using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.SectionSteel.SectionSteelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.SectionSteel.SectionSteelMarket
{
    public class Shanghai : ICrawler
    {
        private const int MarketId = 523;

        //型材
        public void Run()
        {
            GetDataBase.GetData("上海",MarketId,6,"rRihgt");
        }
    }
}
