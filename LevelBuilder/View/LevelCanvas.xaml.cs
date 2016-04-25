using LevelBuilder.Common;
using LevelBuilder.Converters;
using LevelBuilder.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

namespace LevelBuilder.View
{
    /// <summary>
    /// Interaction logic for LevelCanvas.xaml
    /// </summary>
    public partial class LevelCanvas : UserControl
    {

        public LevelCanvas()
        {
            InitializeComponent();
        }



        public PaletteToolType PaletteTool
        {
            get { return (PaletteToolType)GetValue(PaletteToolProperty); }
            set { SetValue(PaletteToolProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PaletteTool.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PaletteToolProperty =
            DependencyProperty.Register("PaletteTool", typeof(PaletteToolType), typeof(LevelCanvas), new PropertyMetadata(PaletteToolType.Selector, OnPaletteToolChanged));

        private static void OnPaletteToolChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var canvas = d as LevelCanvas;
            canvas.tilegrid.CurrentTool = (PaletteToolType)e.NewValue;
        }


        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedLayer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(object), typeof(LevelCanvas), new PropertyMetadata(null));



        public ICollection Layers
        {
            get { return (ICollection)GetValue(LayersProperty); }
            set { SetValue(LayersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Layers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LayersProperty =
            DependencyProperty.Register("Layers", typeof(ICollection), typeof(LevelCanvas), new PropertyMetadata(null, OnLayersChanged));

        private static void OnLayersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var canvas = d as LevelCanvas;
            if (canvas == null || canvas.Layers == null) return;
            canvas.SelectedItem = canvas.Layers.OfType<CanvasLayer>().FirstOrDefault();
        }




        private int _canvasHeight;

        public int CanvasHeight
        {
            get { return _canvasHeight; }
            set { _canvasHeight = value; }
        }

        private int _canvasWidth;

        public int CanvasWidth
        {
            get { return _canvasWidth; }
            set { _canvasWidth = value; }
        }


        public int TileSize
        {
            get { return (int)GetValue(TileSizeProperty); }
            set 
            { 
                SetValue(TileSizeProperty, value); 
            }
        }


  
        // Using a DependencyProperty as the backing store for TileSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TileSizeProperty =
            DependencyProperty.Register("TileSize", typeof(int), typeof(LevelCanvas), new PropertyMetadata(32, OnTileSizeChanged));

        private static void OnTileSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

        }




        public ObservableCollection<ViewModel.PaletteItem> TileBrushes
        {
            get { return (ObservableCollection<ViewModel.PaletteItem>)GetValue(TileBrushProperty); }
            set { SetValue(TileBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TileBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TileBrushProperty =
            DependencyProperty.Register("TileBrushes", typeof(ObservableCollection<ViewModel.PaletteItem>), typeof(LevelCanvas), new PropertyMetadata(null));


   

    }


}
