using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AutoPrice.Crawlers.Base;
using AutoPrice.VMs;
using DataModel;

namespace AutoPrice.Views
{
	/// <summary>
	///     MainWindow.xaml 的交互逻辑
	/// </summary>
	public partial class MainWindow
	{
		public MainWindowVM VM { get; set; }
		private readonly CrawlerManager _cm;

		public MainWindow()
		{
			InitializeComponent();
			VM =new MainWindowVM();
			rootGrid.DataContext = VM;
			_cm = new CrawlerManager();
		}

		private void WindowLoaded(object sender, RoutedEventArgs e)
		{
			
		}

		private void MarketTypeChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				var id = ((MarketType) e.AddedItems[0]).Id;
				VM.LoadMarkets(id);
			}
			else
			{
				VM.ResetMarkets();
			}
			marketList.Items.Refresh();
			e.Handled = true;
		}

		private void MarketChanged(object sender, SelectionChangedEventArgs e)
		{
			if (e.AddedItems.Count > 0)
			{
				var id = ((Market) e.AddedItems[0]).Id;
				VM.LoadNews(id);
			}
			else
			{
				VM.ResetNewses();
			}
			newsList.Items.Refresh();
			e.Handled = true;
		}

		private void ManualRunCrawlerCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
			e.Handled = true;
		}

		private void ManualRunCrawlerExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			var id = (int) e.Parameter;
			var t = new Task(() =>
				                  {
									  List<ICrawler> crawlers = CrawlerFactory.GetInstance().GetCrawlers(id);
									  foreach (var crawler in crawlers)
									  {
										  crawler.Run();
									  }
				                  });
			t.Start();
			
			e.Handled = true;
		}

		private void AddMarket(object sender, RoutedEventArgs e)
		{
			if (VM.SelectedMarketTypeId == 0)
			{
				MessageBox.Show("请先选择市场类型！");
				return;
			}
			(new MarketEditor(VM.SelectedMarketTypeId) {Owner = this}).ShowDialog();
		}

		private void MarketEditCanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.Handled = true;
			e.CanExecute = true;
		}

		private void MarketEditExecuted(object sender, ExecutedRoutedEventArgs e)
		{
			var id = (int) e.Parameter;
			(new MarketEditor(VM.SelectedMarketTypeId, id) { Owner = this }).ShowDialog();
			e.Handled = true;
		}

		private void AddNews(object sender, RoutedEventArgs e)
		{

		}

		private void RunTimerClick(object sender, RoutedEventArgs e)
		{
			try
			{
				if (_cm.IsTimerRunning)
				{
					_cm.End();
					runTimer.Content = "开始自动运行";
				}
				else
				{
					_cm.Begin();
					//runTimer.IsEnabled = false;
					runTimer.Content = "停止自动运行";
				}
				
			}
			catch
			{
			}

		}
	}
}