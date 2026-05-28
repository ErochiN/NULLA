using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace KWIZY
{
    public class TextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isMine = (bool)value;

            if (isMine)
            {
                return Brushes.Black;
            }

            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}