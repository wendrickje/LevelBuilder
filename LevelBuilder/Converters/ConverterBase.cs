using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace LevelBuilder.Converters
{
    
    /// <summary>
    /// Base class for all converters, extends MarkupExtension to enable use of the value converter without making it a static resource
    /// </summary>
    /// <example>
    /// <TextBlock Text="{Binding Name, Converter={local:ConverterBase}}"/>
    /// <TextBlock Text="{Binding Age, Converter={local:ConverterBase}}"/>
    /// </example>
    public abstract class ConverterBase : MarkupExtension, IValueConverter
    {

        /// <summary>
        /// Override of MarkupExtension's ProvideValue
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns>This</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        #region abstract

        /// <summary>
        /// Conversion logic
        /// </summary>
        /// <param name="value">The value being converted</param>
        /// <param name="targetType">Property type</param>
        /// <param name="parameter">Converter parameter</param>
        /// <param name="culture">Current culture</param>
        /// <returns></returns>
        public abstract object DoConvert(object value, Type targetType, object parameter, CultureInfo culture);


        #endregion

        #region virtual methods


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return the value of the abstract method DoConvert
            return DoConvert(value, targetType, parameter, culture);
        }

        /// <summary>
        /// Optional ConvertBack method. By default returns value;
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
        #endregion
    }
}
