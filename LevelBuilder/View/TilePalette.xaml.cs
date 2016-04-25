using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LevelBuilder.Common;
using LevelBuilder.ViewModel;

namespace LevelBuilder.View
{
    /// <summary>
    /// Interaction logic for TilePalette.xaml
    /// </summary>
    public partial class TilePalette : UserControl
    {
        public TilePalette()
        {
            InitializeComponent();
        }




        public PaletteToolType SelectedTool
        {
            get { return (PaletteToolType)GetValue(SelectedToolProperty); }
            set { SetValue(SelectedToolProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTool.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedToolProperty =
            DependencyProperty.Register("SelectedTool", typeof(PaletteToolType), typeof(TilePalette), new PropertyMetadata(PaletteToolType.Selector, OnSelectedToolChanged));

        static void OnSelectedToolChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var palette = sender as TilePalette;
            if (palette == null) return;
            ((LevelBuilder.ViewModel.Project)palette.DataContext).LevelConfiguration.Palette.CurrentTool = palette.SelectedTool;
        }

        public ObservableCollection<ViewModel.PaletteItem> SelectedItems
        {
            get { return (ObservableCollection<ViewModel.PaletteItem>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<ViewModel.PaletteItem>), typeof(TilePalette), new PropertyMetadata(null, SelectedItemsPropertyChanged));

        private static void SelectedItemsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var tilepalette = sender as TilePalette;

            if (e.OldValue != null)
            {
                var old = (INotifyCollectionChanged)e.OldValue;
                //unsubscribe 
                old.CollectionChanged -= tilepalette.SelectedItemsCollectionChanged;
            }

            if (e.NewValue != null)
            {
                var coll = (ObservableCollection<ViewModel.PaletteItem>)e.NewValue;

                coll.CollectionChanged += tilepalette.SelectedItemsCollectionChanged;
               
            }
        }

        void SelectedItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null) return;
            var selected = e.NewItems.Cast<ViewModel.PaletteItem>().First();
            if(SelectedItems.Any(p => p.Key == selected.Key)) return;
            var currentitems = SelectedItems;
            currentitems.Add(selected);
            SelectedItems = new ObservableCollection<ViewModel.PaletteItem>(currentitems);
        }



        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(TilePalette), new PropertyMetadata(new DataTemplate()));




        private ObservableCollection<TilePaletteItem> _selectedItemContainers = new ObservableCollection<TilePaletteItem>();

        public ObservableCollection<TilePaletteItem> SelectedItemContainers
        {
            get { return _selectedItemContainers; }
        }

        public ICollection ItemsSource
        {
            get { return (ICollection)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }


        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ICollection), typeof(TilePalette), new PropertyMetadata(null, OnItemsSourceInitalized));

        private static void OnItemsSourceInitalized(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
        }


        private bool _ismousedown = false;
        private bool _wasselecting = false;
        private bool _isselecting = false;

        List<Key> _keys = new List<Key> { Key.LeftShift, Key.RightShift, Key.LeftCtrl, Key.RightCtrl };
        private Key? GetExtenderKeyDown()
        {
            var k = _keys.Find(key => Keyboard.GetKeyStates(key).HasFlag(KeyStates.Down));
            return k == Key.None ? (Key?)null : k;

        }

        private void AddSelectedItem(TilePaletteItem item)
        {
            SelectedItemContainers.Add(item);
            item.IsSelected = true;
            if (SelectedItems == null) return;
            SelectedItems.Add(item.Content as ViewModel.PaletteItem);

        }

        private void ClearSelectedItems()
        {

            SelectedItemContainers.ToList().ForEach(i => i.IsSelected = false);
            SelectedItemContainers.Clear();
            if (SelectedItems == null) return;
            SelectedItems.Clear();
        }

        private void TilePaletteItem_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            StartTileBrush.IsChecked = false;
            
            EventTileBrush.IsChecked = false;

            var key = GetExtenderKeyDown();
            var iskeydown = key.HasValue;
            if (_wasselecting && !iskeydown)
            {
                _wasselecting = false;
                ClearSelectedItems();
            }
            _ismousedown = true;
            var item = sender as TilePaletteItem;
            if (item == null) return;
            AddSelectedItem(item);
            _isselecting = true;

            /*      3   F
             *      2   F
             *      1   x
             *      0
             * 
             */

            //takes the latest click (sender) 
            //and takes the 2nd most recent click
            //based on these two item's positions
            //select all cells in between using x and y
            //it will clear all selections that are outside these bounds
            //example above:
            // 0 - 3 are previous clicks, 0 being the first 3 being the most recent
            // x is the target click, the sender
            // F are the cells to be filled
            // 0 would get unselected
            //the final selections would be 1,2,3,f,f,x
            if (!iskeydown || SelectedItemContainers.Count < 2) return;
            if (key == Key.LeftCtrl || key == Key.RightCtrl) return;

            //get the previous item
            var previousitem = SelectedItemContainers[SelectedItemContainers.Count - 2];
            var previousitemcellscontainer = Utilities.GetAncestorFromType(previousitem, typeof(TilePaletteCellCollection), false) as TilePaletteCellCollection;
            var previousitemrowcontainer = Utilities.GetAncestorFromType(previousitem, typeof(TilePaletteRow), false);
            var previousitemrow = paletteitems.ItemContainerGenerator.IndexFromContainer(previousitemrowcontainer);
            var previousitemcolumn = previousitemcellscontainer.ItemContainerGenerator.IndexFromContainer(previousitem);

            var currentitem = item;
            var currentitemcellscontainer = Utilities.GetAncestorFromType(currentitem, typeof(TilePaletteCellCollection), false) as TilePaletteCellCollection;
            var currentitemrowcontainer = Utilities.GetAncestorFromType(currentitem, typeof(TilePaletteRow), false);
            var currentitemrow = paletteitems.ItemContainerGenerator.IndexFromContainer(currentitemrowcontainer);
            var currentitemcolumn = currentitemcellscontainer.ItemContainerGenerator.IndexFromContainer(currentitem);

            var xmin = Math.Min(previousitemcolumn, currentitemcolumn);
            var ymin = Math.Min(previousitemrow, currentitemrow);

            var xmax = Math.Max(previousitemcolumn, currentitemcolumn);
            var ymax = Math.Max(previousitemrow, currentitemrow);

            ClearSelectedItems();
            for (int y = ymin; y <= ymax; y++)
            {

                var row = paletteitems.ItemContainerGenerator.ContainerFromIndex(y) as TilePaletteRow;
                if (row == null) continue;

                var columns = row.Template.FindName("tilepalettecellcollection", row) as TilePaletteCellCollection;
                if (columns == null) continue;

                for (int x = xmin; x <= xmax; x++)
                {
                    var cell = columns.ItemContainerGenerator.ContainerFromIndex(x) as TilePaletteItem;
                    if (cell == null) continue;
                    AddSelectedItem(cell);
                }
            }

        }
        private void TilePaletteItem_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _wasselecting = true;
            _ismousedown = false;
            _isselecting = false;
        }

        private void TilePaletteItem_OnMouseOver(object sender, MouseEventArgs e)
        {
            if (!_ismousedown && !_isselecting) return;
            TilePaletteItem_OnMouseDown(sender, null);
        }

        private void Paletteitems_OnMouseLeave(object sender, MouseEventArgs e)
        {
            _wasselecting = true;
            _ismousedown = false;
            _isselecting = false;
        }



        private void Picker_Checked(object sender, RoutedEventArgs e)
        {
            SelectedTool = PaletteToolType.Selector;

        }

        private void StartTileBrush_OnChecked(object sender, RoutedEventArgs e)
        {
            ClearSelectedItems();
            var item = new TilePaletteItem()
            {
                Key = Common.ResourceKeys.Global.StartTileKey,
                Content = new ViewModel.PaletteItem(-1, -1, -1, null, Common.ResourceKeys.Global.StartTileKey)
            };
            AddSelectedItem(item);
        }

        private void EventTileBrush_OnChecked(object sender, RoutedEventArgs e)
        {
            ClearSelectedItems();
            var item = new TilePaletteItem()
            {
                Key = Common.ResourceKeys.Global.EventTileKey,
                Content = new ViewModel.PaletteItem(-1, -1, -1, null, Common.ResourceKeys.Global.EventTileKey)
            };
            AddSelectedItem(item);
        }



        private void EmptyTileBrush_Checked(object sender, RoutedEventArgs e)
        {
            SelectedTool = PaletteToolType.Eraser;
            //ClearSelectedItems();
            //var item = new TilePaletteItem()
            //{
            //    Key = Common.ResourceKeys.Global.EmptyTileKey,//eraser
            //    Content = new ViewModel.PaletteItem(-1, -1, -1, null, Common.ResourceKeys.Global.EmptyTileKey)
            //};
            //AddSelectedItem(item);
        }

        private void BrushTool_Checked(object sender, RoutedEventArgs e)
        {
            SelectedTool = PaletteToolType.Brush;
            //clear the eraser brush when clicked
            if(SelectedItemContainers.Any(i => i.Key == Common.ResourceKeys.Global.EmptyTileKey))
                ClearSelectedItems();
        }

        private void FillTool_Checked(object sender, RoutedEventArgs e)
        {
            SelectedTool = PaletteToolType.Bucket;
            //clear the eraser brush when clicked
            if (SelectedItemContainers.Any(i => i.Key == Common.ResourceKeys.Global.EmptyTileKey))
                ClearSelectedItems();

        }
    }
}
