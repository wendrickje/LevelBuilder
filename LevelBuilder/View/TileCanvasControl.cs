using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace LevelBuilder.View
{


    public class TileCanvasCell : ContentControl
    {


        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSelected.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.Register("IsSelected", typeof(bool), typeof(TileCanvasCell), new PropertyMetadata(false));
    }

    public class TileCanvasCellCollection : ItemsControl
    {
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is TileCanvasCell);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TileCanvasCell();
        }



    }

    public class TileCanvasRowCollection : ItemsControl
    {
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return (item is TileCanvasRow);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TileCanvasRow();
        }
    }

    public class TileCanvasRow : ContentControl { }
}
