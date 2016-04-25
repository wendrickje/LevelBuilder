using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelBuilder.Converters
{
    public class LayerLabelConverter : ConverterBase
    {
        public LayerLabelConverter()
        {

        }

        public override object DoConvert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(!(value is int)) return value;
            var label = GetLayerLabel((int)value);
            return label;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetLayerLabel(int index)
        {
            switch (index)
            {
                case 3:
                    return Common.Constants.LayerLabel_Backdrop;
                case 2:
                    return Common.Constants.LayerLabel_Background;
                case 0:
                    return Common.Constants.LayerLabel_Middleground;
                case 1:
                    return Common.Constants.LayerLabel_Foreground;
            }
            return null;
        }
    }
}
