using System;
using System.Windows;
using System.Windows.Controls;
using AutoPrice.VMs;

namespace AutoPrice.Views
{
	/// <summary>
	/// MarketEditor.xaml 的交互逻辑
	/// </summary>
	public partial class MarketEditor
	{
		public MarketEditorVM VM { get; set; }

		public MarketEditor(int marketTypeId, int marketId = 0)
		{
			InitializeComponent();
			VM = new MarketEditorVM(marketTypeId, marketId);
			rootGrid.DataContext = VM;
		}

		private void Save(object sender, RoutedEventArgs e)
		{
			try
			{
				if (Validation.GetHasError(crmIdText))
				{
					MessageBox.Show("CRM ID 输入有误");
					return;
				}
				VM.Save();
				((MainWindow)Owner).VM.RefreshMarketList();
				((MainWindow)Owner).marketList.Items.Refresh();
				Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			
		}
	}
}
