
// BubbleColorConverter.cs

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace KWIZY
{
    public class BubbleColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isMine = (bool)value;

            if (isMine)
            {
                return new SolidColorBrush(
                    (Color)ColorConverter.ConvertFromString("#00ff89"));
            }

            return new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString("#374151"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
