using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LevelBuilder.Converters;

namespace LevelBuilder.Common
{
    /// <summary>
    /// Interaction logic for LoadingIndicator.xaml
    /// </summary>
    public partial class LoadingIndicator : UserControl
    {
        public LoadingIndicator()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 
        /// </summary>
        public bool IsLoading
        {
            get { return (bool)GetValue(IsLoadingProperty); }
            set { SetValue(IsLoadingProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register("IsLoading", typeof(bool), typeof(LoadingIndicator), new PropertyMetadata(false));


        /// <summary>
        /// 
        /// </summary>
        public IndicatorType IndicatorType
        {
            get { return (IndicatorType)GetValue(IndicatorTypeProperty); }
            set { SetValue(IndicatorTypeProperty, value); }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IndicatorTypeProperty =
            DependencyProperty.Register("IndicatorType", typeof(IndicatorType), typeof(LoadingIndicator), new PropertyMetadata(IndicatorType.Mini));


    }

    /// <summary>
    /// 
    /// </summary>
    public enum IndicatorType
    {
        Mini,
        Enlarged

    }

    public class IndicatorTypeVisibilityConverter : ConverterBase
    {
        public IndicatorTypeVisibilityConverter()
            : base()
        {
        }


        public override object DoConvert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (!(value is IndicatorType) || !(parameter is IndicatorType))
                return Visibility.Collapsed;
            return (IndicatorType)value == (IndicatorType)parameter ? Visibility.Visible : Visibility.Collapsed;
        }


    }
}
