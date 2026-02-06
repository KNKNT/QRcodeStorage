using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace QRcodeStorage.Services
{
    public class NullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            value = string.IsNullOrEmpty(value.ToString()) ? "–" : value;
            return value;
        }

        public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            value = value.ToString() == "–" ? null : value;
            return value;
        }
    }
}
