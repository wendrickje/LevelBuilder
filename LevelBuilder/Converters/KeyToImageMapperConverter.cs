using LevelBuilder.Common;
using LevelBuilder.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LevelBuilder.Converters
{
    public class KeyToImageMapperConverter : ConverterBase
    {
        public KeyToImageMapperConverter() { }

        readonly ImageSourceConverter _converter = new ImageSourceConverter();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object DoConvert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            //var cell = value as DataGridCell;
            //if (cell == null) return value;
            //var datagrid = Utilities.GetAncestorFromType(cell, typeof(DataGrid), false) as DataGrid;
            //var dataview = datagrid.ItemsSource as DataView;
            //var rowcontrol = Utilities.GetAncestorFromType(cell, typeof(DataGridRow), false) as DataGridRow;
            //var datarow = rowcontrol.DataContext as DataRowView;

            //var row = dataview.Table.Rows.IndexOf(datarow.Row);

            //var col = cell.Column.DisplayIndex;

            var key = value as int?;
            if (!key.HasValue) return value;

            PaletteItem paletteitem;
            var succeed = Mapper.Mappings.TryGetValue(key.Value, out paletteitem);

            return succeed ? _converter.DoConvert(paletteitem.Bitmap, null, null, null) : value;
        }
    }
}
