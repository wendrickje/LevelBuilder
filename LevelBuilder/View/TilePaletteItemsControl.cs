using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace LevelBuilder.View
{

    public class TilePaletteCellCollection : ItemsControl
    {
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is TilePaletteItem);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TilePaletteItem();
        }

    }

    public class TilePaletteRowCollection : ItemsControl
    {
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is TilePaletteRow);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TilePaletteRow();
        }

        
    }


    public class TilePaletteRow : ContentControl { }
}
