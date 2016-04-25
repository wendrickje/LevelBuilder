using LevelBuilder.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LevelBuilder.Converters
{
    public class CellContentConverter : ConverterBase
    {
        public override object DoConvert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var cell = value as DataGridCell;
            if (cell == null) return value; 
            var datagrid = Utilities.GetAncestorFromType(cell, typeof(DataGrid), false) as DataGrid;
            var dataview = datagrid.ItemsSource as DataView;
            var rowcontrol = Utilities.GetAncestorFromType(cell, typeof(DataGridRow), false) as DataGridRow;
            var datarow = rowcontrol.DataContext as DataRowView;

            var row = dataview.Table.Rows.IndexOf(datarow.Row);

            var col = cell.Column.DisplayIndex;

            return dataview.Table.Rows[row][col];


        }
    }
}
