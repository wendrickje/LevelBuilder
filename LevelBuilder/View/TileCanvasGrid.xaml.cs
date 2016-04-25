using LevelBuilder.Common;
using LevelBuilder.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LevelBuilder.View
{
    /// <summary>
    /// Interaction logic for TileCanvasGrid.xaml
    /// </summary>
    public partial class TileCanvasGrid : UserControl
    {
        public TileCanvasGrid()
        {
            InitializeComponent();
        }



        public PaletteToolType CurrentTool
        {
            get { return (PaletteToolType)GetValue(CurrentToolProperty); }
            set { SetValue(CurrentToolProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentTool.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentToolProperty =
            DependencyProperty.Register("CurrentTool", typeof(PaletteToolType), typeof(TileCanvasGrid), new PropertyMetadata(PaletteToolType.Selector));




        public DataTemplateSelector ItemTemplateSelector
        {
            get { return (DataTemplateSelector)GetValue(ItemTemplateSelectorProperty); }
            set { SetValue(ItemTemplateSelectorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplateSelector.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateSelectorProperty =
            DependencyProperty.Register("ItemTemplateSelector", typeof(DataTemplateSelector), typeof(TileCanvasGrid), new PropertyMetadata(null));




        public ObservableCollection<ObservableCollection<int>> ItemsSource
        {
            get { return (ObservableCollection<ObservableCollection<int>>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<ObservableCollection<int>>),
            typeof(TileCanvasGrid), new PropertyMetadata(new ObservableCollection<ObservableCollection<int>>()));





        public int RowHeight
        {
            get { return (int)GetValue(RowHeightProperty); }
            set { SetValue(RowHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for RowHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowHeightProperty =
            DependencyProperty.Register("RowHeight", typeof(int), typeof(TileCanvasGrid), new PropertyMetadata(32));




        public int ColumnWidth
        {
            get { return (int)GetValue(ColumnWidthProperty); }
            set { SetValue(ColumnWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ColumnWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnWidthProperty =
            DependencyProperty.Register("ColumnWidth", typeof(int), typeof(TileCanvasGrid), new PropertyMetadata(32));



        public ObservableCollection<ViewModel.PaletteItem> Brushes
        {
            get { return (ObservableCollection<ViewModel.PaletteItem>)GetValue(BrushProperty); }
            set { SetValue(BrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Brush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BrushProperty =
            DependencyProperty.Register("Brushes", typeof(ObservableCollection<ViewModel.PaletteItem>), typeof(TileCanvasGrid), new PropertyMetadata(null));





        private bool _ismousedown = false;
        private bool _wasselecting = false;
        private bool _isselecting = false;

        List<Key> _keys = new List<Key> { Key.LeftShift, Key.RightShift, Key.LeftCtrl, Key.RightCtrl };
        private Key? GetExtenderKeyDown()
        {
            var k = _keys.Find(key => Keyboard.GetKeyStates(key).HasFlag(KeyStates.Down));
            return k == Key.None ? (Key?)null : k;

        }




        private void TileCanvasCell_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _wasselecting = true;
            _ismousedown = false;
            _isselecting = false;
        }

        List<TileCanvasCell> _selectedCells;

        public List<TileCanvasCell> SelectedCells { get { return _selectedCells ?? (_selectedCells = new List<TileCanvasCell>()); } }


        private void AddSelectedItem(TileCanvasCell item)
        {
            SelectedCells.Add(item);
            item.IsSelected = true;
        }

        private void ClearSelectedItems()
        {
            SelectedCells.ForEach(i => i.IsSelected = false);
            SelectedCells.Clear();
        }


        private void TileCanvasCell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var key = GetExtenderKeyDown();
            var iskeydown = key.HasValue;
            if (_wasselecting && !iskeydown)
            {
                _wasselecting = false;
                ClearSelectedItems();
            }
            _ismousedown = true;
            var cell = sender as TileCanvasCell;
            if (cell == null) return;
            //AddSelectedItem(cell);
            _isselecting = true;



            var cellcollection =
                Utilities.GetAncestorFromType(cell, typeof(TileCanvasCellCollection), false) as
                    TileCanvasCellCollection;
            var row = Utilities.GetAncestorFromType(cell, typeof(TileCanvasRow), false) as TileCanvasRow;
            var rowcollection =
                Utilities.GetAncestorFromType(cell, typeof(TileCanvasRowCollection), false) as TileCanvasRowCollection;

            if (row == null || cellcollection == null || rowcollection == null) return;

            var rowindex = rowcollection.ItemContainerGenerator.IndexFromContainer(row);
            var columnindex = cellcollection.ItemContainerGenerator.IndexFromContainer(cell);


            switch (CurrentTool)
            {
                case PaletteToolType.Eraser:
                    {
                        ItemsSource[rowindex][columnindex] = Common.ResourceKeys.Global.EmptyTileKey;
                        return;
                    }
                case PaletteToolType.Selector:
                    {
                        AddSelectedItem(cell);
                        return;
                    }
            }


            //dont do the heavy stuff if its only 1 item
            if (Brushes.Count == 1)
            {
                var brush = Brushes.First();

                switch (CurrentTool)
                {
                    case PaletteToolType.Brush:
                        ItemsSource[rowindex][columnindex] = brush.Key;
                        break;
                    case PaletteToolType.Bucket:
                        //put this on a thread
                        //fillloader.Visibility = Visibility.Visible;
                        FloodFill(new Node(columnindex, rowindex, ItemsSource[rowindex][columnindex]), ItemsSource[rowindex][columnindex], brush.Key);
                        //FloodFill(rowindex, columnindex, ItemsSource[rowindex][columnindex], ItemsSource[rowindex][columnindex], brush.Key);
                        //using(var worker = new BackgroundWorker())
                        //{
                        //    worker.DoWork += ((w, args) =>
                        //    {
                        //        FloodFill(new Node(rowindex, columnindex, ItemsSource[rowindex][columnindex]), ItemsSource[rowindex][columnindex], brush.Key);
                        //    });
                        //    worker.ProgressChanged += ((p, args) => 
                        //    {
 
                        //    });
                        //    worker.RunWorkerCompleted += ((c, args) =>
                        //    {
                        //        fillloader.Visibility = Visibility.Collapsed;
                        //    });
                        //    worker.WorkerReportsProgress = true;
                        //    worker.RunWorkerAsync();
                        //}
                        break;
                }


            }
            else
            {

                var maxrowindex = ItemsSource.Count - 1;
                var maxcolumnindex = ItemsSource[0].Count - 1;

                switch (CurrentTool)
                {
                    case PaletteToolType.Brush:
                        {
                            //start with the selected cell and loop through the brushes
                            //use the difference of the x index of the brushes to know how many times to loop x
                            //same for y

                            var startx = Brushes.Min(b => b.XIndex);
                            var endx = Brushes.Max(b => b.XIndex);
                            var starty = Brushes.Min(b => b.YIndex);
                            var endy = Brushes.Max(b => b.YIndex);
                            var loopsx = columnindex;
                            var loopsy = rowindex;

                            for (int y = starty; y <= endy; y++)
                            {
                                if (loopsy > maxrowindex) break;//if we reached the last row dont bother to paint
                                for (int x = startx; x <= endx; x++)
                                {
                                    if (loopsx > maxcolumnindex) break;//if we reached the last column dont bother to paint

                                    var brush = Brushes.FirstOrDefault(b => b.XIndex == x && b.YIndex == y);
                                    if (brush == null) { loopsx++; continue; }//if theres no brush matching the current x and y then ignore this cell on the canvas

                                    ItemsSource[loopsy][loopsx] = brush.Key;

                                    loopsx++;

                                }
                                loopsx = columnindex; //reset to the first column
                                loopsy++;
                            }

                            break;
                        }

                    case PaletteToolType.Bucket: //todo: handle bucket
                        break;
                }

            }

        }

        private void TileCanvasCell_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!_ismousedown && !_isselecting) return;
            TileCanvasCell_MouseLeftButtonDown(sender, null);
        }

        private void baselistbox_MouseLeave(object sender, MouseEventArgs e)
        {
            _wasselecting = true;
            _ismousedown = false;
            _isselecting = false;
        }

        #region tool code

        // 1. Set Q to the empty queue.
        // 2. If the color of node is not equal to target-color, return.
        // 3. Add node to Q.
        // 4. For each element N of Q:
        // 5.     If the color of N is equal to target-color:
        // 6.         Set w and e equal to N.
        // 7.         Move w to the west until the color of the node to the west of w no longer matches target-color.
        // 8.         Move e to the east until the color of the node to the east of e no longer matches target-color.
        // 9.         For each node n between w and e:
        //10.             Set the color of n to replacement-color.
        //11.             If the color of the node to the north of n is target-color, add that node to Q.
        //12.             If the color of the node to the south of n is target-color, add that node to Q.
        //13. Continue looping until Q is exhausted.
        //14. Return.

        void FloodFill(Node N, int target, int replacement)
        {
            List<Node> nodes = new List<Node>();
            int _topmost = 0;
            int _rightmost = ItemsSource[0].Count - 1;
            int _bottommost = ItemsSource.Count - 1;
            int _leftmost = 0;
            var q = new Queue<Node>();
            if (N.Content != target) return;
            q.Enqueue(N);
            
            while(q.Any())
            {
                var node = q.Dequeue();
                if (node.Content == target)
                {
                    //move west
                    var westnode = new Node(node.X, node.Y, node.Content);
                    while (IsWithinBounds(westnode.X, westnode.Y, _topmost, _rightmost, _bottommost, _leftmost) && ItemsSource[westnode.Y][westnode.X] == target)
                    {
                        nodes.Add(westnode);
                        if (!IsWithinBounds(westnode.X - 1, westnode.Y, _topmost, _rightmost, _bottommost, _leftmost)) break;
                        westnode = new Node(westnode.X - 1, westnode.Y, ItemsSource[westnode.Y][westnode.X - 1]);
                    }
                    //move east
                    if (IsWithinBounds(node.X + 1, node.Y, _topmost, _rightmost, _bottommost, _leftmost))
                    {
                        var eastnode = new Node(node.X + 1, node.Y, ItemsSource[node.Y][node.X + 1]);
                        while (ItemsSource[eastnode.Y][eastnode.X] == target)
                        {
                            nodes.Add(eastnode);
                            if (!IsWithinBounds(eastnode.X + 1, eastnode.Y, _topmost, _rightmost, _bottommost, _leftmost)) break;
                            eastnode = new Node(eastnode.X + 1, eastnode.Y, ItemsSource[eastnode.Y][eastnode.X + 1]);
                        }
                    }
                    foreach(var _n in nodes)
                    {
                        ItemsSource[node.Y][_n.X] = replacement;
                        _n.Content = replacement;

                        //check north
                        if (IsWithinBounds(_n.X, node.Y - 1, _topmost, _rightmost, _bottommost, _leftmost))
                        {
                            var northnode = new Node(_n.X, node.Y - 1, ItemsSource[node.Y - 1][_n.X]);
                            q.Enqueue(northnode);
                        }
                        //check south
                        if (IsWithinBounds(_n.X, node.Y + 1, _topmost, _rightmost, _bottommost, _leftmost))
                        {
                            var southnode = new Node(_n.X, node.Y + 1, ItemsSource[node.Y + 1][_n.X]);
                            q.Enqueue(southnode);
                        }
                        
                    }
                    nodes.Clear();
                }
            }
        }
        
        bool IsWithinBounds(int x, int y, int topmost, int rightmost, int bottommost, int leftmost)
        {
            return x <= rightmost && x >= leftmost && y <= bottommost && y >= topmost;
        }

        void FloodFill(int x, int y, int current, int target, int replacement)
        {

            if (target == replacement) return;
            if (current != target) return;
            //Set the color of node to replacement-color
            ItemsSource[x][y] = replacement;
            

            int _topmost = 0;
            int _rightmost = ItemsSource[0].Count - 1;
            int _bottommost = ItemsSource.Count - 1;
            int _leftmost = 0;

            //go north
            if (y - 1 > _topmost)
            {
                FloodFill(x, y - 1, ItemsSource[x][y - 1], target, replacement);
            }

            //go east
            if (x + 1 < _rightmost)
            {
                FloodFill(x + 1, y, ItemsSource[x + 1][y], target, replacement);
            }

            //go south
            if (y + 1 < _bottommost)
            {
                FloodFill(x, y + 1, ItemsSource[x][y + 1], target, replacement);
            }

            //go west
            if (x - 1 > _leftmost)
            {
                FloodFill(x - 1, y, ItemsSource[x - 1][y], target, replacement);
            }

            return;
        }
        #endregion


    }

    internal class Node
    {
        internal Node(int x, int y, int content)
        {
            X = x;
            Y = y;
            Content = content;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Content { get; set; }
    }
}
