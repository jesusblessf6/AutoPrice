using AutoPrice.Crawlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Silicon.Silicon
{
     public class GZNoOrientation : ICrawler
    {
         public void Run()
         {
             SiliconBase.SiliconBase.GetData("a_div3", "广州市场无取向硅钢行情", "广州");
         }
    }
}
