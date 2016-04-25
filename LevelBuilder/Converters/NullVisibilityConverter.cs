using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LevelBuilder.Converters
{
    public class NullVisibilityConverter : ConverterBase
    {
        public NullVisibilityConverter() { }

        public override object DoConvert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }
    }
}
