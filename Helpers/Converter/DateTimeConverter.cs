using Monday;
using System;
using System.Windows.Data;

namespace Helpers.Converter
{
    public class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string date = string.Empty;
            if (value is DateTime)
            {
                DateTime dt = (DateTime)value;
                if (parameter.ToString() == "MonDateTime")
                    date = dt.MonDateTime();
                else
                    date = dt.MonDate();
                return (date);
            }
            return date;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
