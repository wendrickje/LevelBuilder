using System;
using System.Drawing;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace LevelBuilder.Converters
{
    public class ImageSourceConverter : ConverterBase
    {
        public ImageSourceConverter()
        {

        }

        /// <summary>
        /// takes in a bitmap and returns a wpf image source
        /// </summary>
        /// <param name="value">a Bitmap</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>BitmapSource</returns>
        public override object DoConvert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var bitmap = value as Bitmap;
            if (bitmap == null) return value;
            var hbitmap = bitmap.GetHbitmap();
            var source = Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            
            return source;
        } 

    }
}
