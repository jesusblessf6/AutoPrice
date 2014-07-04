using System.ComponentModel;
using System.Windows.Threading;

namespace AutoPrice.VMs
{
	public abstract class BaseVM : DispatcherObject, INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void Notify(string propName)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(propName));
			}
		}
	}
}
