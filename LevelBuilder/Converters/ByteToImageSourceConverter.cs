using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace LevelBuilder.Converters
{

    public class ByteToImageSourceConverter : ConverterBase
    {
        public ByteToImageSourceConverter() { }

        private readonly ImageConverter _internalconverter = new ImageConverter();

        public override object DoConvert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var bytes = value as byte[];
            if (bytes == null) return value;
            var bitmap = _internalconverter.ConvertFrom(bytes) as Bitmap;
            var hbitmap = bitmap.GetHbitmap();
            var source = Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            return source;
        }
    }

    public class ByteToBitmapConverter : ConverterBase
    {
        public ByteToBitmapConverter() { }


        private readonly ImageConverter _internalconverter = new ImageConverter();

        public override object DoConvert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            var bytes = value as byte[];
            if (bytes == null) return value;
            return _internalconverter.ConvertFrom(bytes) as Bitmap;
        }
    }

    public class BitmapToByteConverter : ConverterBase
    {
        public BitmapToByteConverter() { }

        private readonly ImageConverter _internalconverter = new ImageConverter();

        public override object DoConvert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            var bitmap = value as Bitmap;
            if (bitmap == null) return value;
            return _internalconverter.ConvertTo(bitmap, typeof(byte[])) as byte[];
        }
    }
}
