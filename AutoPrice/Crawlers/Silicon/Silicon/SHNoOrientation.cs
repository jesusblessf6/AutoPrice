using AutoPrice.Crawlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Silicon.Silicon
{
     public class SHNoOrientation : ICrawler
    {
         public void Run()
         {
             SiliconBase.SiliconBase.GetData("a_div0", "上海市场无取向硅钢价格行情", "上海");
         }
    }
}
