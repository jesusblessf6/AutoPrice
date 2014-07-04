using AutoPrice.Crawlers.Base;
using AutoPrice.Crawlers.SMMBase;

namespace AutoPrice.Crawlers.MetalSpot
{
	public class SHSpot : ICrawler
	{
		private const int MarketId = 29;

		public void Run()
		{
			SMMBaseClass.ResolvePage(MarketId, "http://www.smm.cn/shtoday.php");//上海现货
		}
	}
}
