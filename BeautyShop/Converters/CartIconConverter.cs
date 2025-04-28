using System.Globalization;
using Microsoft.Maui.Controls;

namespace BeautyShop.Converters
{
    public class CartIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isInCart = (bool)value;
            return isInCart ? "cart_filled.png" : "cart_outline.png"; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}
