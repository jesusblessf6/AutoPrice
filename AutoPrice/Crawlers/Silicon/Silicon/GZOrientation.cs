using AutoPrice.Crawlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Silicon.Silicon
{
     public class GZOrientation : ICrawler
    {
         public void Run()
         {
             SiliconBase.SiliconBase.GetData("a_div3", "广州市场取向硅钢价格行情", "广州");
         }
    }
}
