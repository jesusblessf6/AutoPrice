using System.Collections.Generic;
using System.Linq;
using DataModel;

namespace AutoPrice.VMs
{
	public class MainWindowVM : BaseVM
	{
		private List<MarketType> _marketTypes;
		private List<Market> _markets;
		private List<News> _newses;
		private int _selectedMarketTypeId;
		private int _selectedMarketId;

		public List<MarketType> MarketTypes
		{
			get { return _marketTypes; }
			set 
			{
				_marketTypes = value;
				Notify("MarketTypes");
			}
		} 

		public List<Market> Markets
		{
			get { return _markets; }
			set
			{
				_markets = value;
				Notify("Markets");
			}
		}

		public List<News> Newses
		{
			get { return _newses; }
			set { _newses = value; Notify("Newses"); }
		} 

		public int SelectedMarketTypeId
		{
			get { return _selectedMarketTypeId; }
			set
			{
				if (_selectedMarketTypeId != value)
				{
					_selectedMarketTypeId = value;
					Notify("SelectedMarketTypeId");
				}
			}
		}

		public int SelectedMarketId
		{
			get { return _selectedMarketId; }
			set
			{
				if (_selectedMarketId != value)
				{
					_selectedMarketId = value;
					Notify("SelectedMarketId");
				}
			}
		}

		public MainWindowVM()
		{
			using (var ctx = new AutoNewsEntities())
			{
				_marketTypes = ctx.MarketTypes.ToList();
				_markets = new List<Market>();
				_newses = new List<News>();
			}
		}

		public void LoadMarkets(int marketTypeId)
		{
			using (var ctx = new AutoNewsEntities())
			{
				Markets = ctx.Markets.Where(o => o.MarketTypeId == marketTypeId).ToList();
			}
		}

		public void LoadNews(int marketId)
		{
			using (var ctx = new AutoNewsEntities())
			{
				Newses = ctx.News.Where(o => o.MarketId == marketId).ToList();
			}
		}

		public void ResetMarkets()
		{
			Markets.Clear();
			SelectedMarketTypeId = -1;
		}

		public void ResetNewses()
		{
			Newses.Clear();
			SelectedMarketId = -1;
		}

		public void RefreshMarketList()
		{
			LoadMarkets(_selectedMarketTypeId);
		}
	}
}