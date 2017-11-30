using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace ProCalendar.UI.ValueConverters
{
    public class DateTimeToMonth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) =>
            String.Format("{0:Y}", (DateTime)value);

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            String.IsNullOrEmpty(value as String) ? DateTime.MinValue : DateTime.Parse(value as String);
    }
}
