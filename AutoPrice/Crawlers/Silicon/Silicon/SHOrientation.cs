using AutoPrice.Crawlers.Base;
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

namespace AutoPrice.Crawlers.Silicon.Silicon
{
     public class SHOrientation : ICrawler
    {

         public void Run()
         {
             SiliconBase.SiliconBase.GetData("a_div0", "上海市场取向硅钢价格行情", "上海");
         }
    }
}
