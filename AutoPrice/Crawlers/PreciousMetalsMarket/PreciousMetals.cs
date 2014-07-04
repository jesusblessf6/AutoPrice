using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.SMMBase;

namespace AutoPrice.Crawlers.PreciousMetalsMarket
{
	public class PreciousMetals : ICrawler
	{
		private const int MarketId = 28;

		public void Run()
		{
			SMMBaseClass.ResolvePage(MarketId, "http://www.smm.cn/shtoday.php/2");//贵金属
		}
	}
}
