using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace acheesporte_locator_app.Converters
{
    public class PaidColorConverter : IValueConverter
    {
        public Color PaidColor { get; set; } = Colors.Green;
        public Color UnpaidColor { get; set; } = Colors.Red;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isPaid)
                return isPaid ? PaidColor : UnpaidColor;

            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
