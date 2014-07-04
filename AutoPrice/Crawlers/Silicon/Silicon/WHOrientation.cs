using AutoPrice.Crawlers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPrice.Crawlers.Silicon.Silicon
{
     public class WHOrientation : ICrawler
    {
         public void Run()
         {
             SiliconBase.SiliconBase.GetData("a_div4", "武汉市取向场硅钢价格行情","武汉");
         }
    }
}
