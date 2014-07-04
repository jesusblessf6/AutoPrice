using System;
using System.Globalization;
using System.Windows.Data;

namespace AutoPrice.Converters
{
	[ValueConversion(typeof(TimeSpan?), typeof(string))]
	public class TimeSpan2String : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return string.Empty;
			}

			var t = ((TimeSpan?) value).Value;
			return t.Hours.ToString() + ":" + t.Minutes.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				return null;
			}

			TimeSpan ts;
			if (TimeSpan.TryParse((string) value, out ts))
			{
				return ts;
			}
			
			return null;
		}
	}
}
