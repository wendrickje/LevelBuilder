using LevelBuilder.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace LevelBuilder.ViewModel
{
    public class Canvas : Core
    {
        public Canvas(int canvasheight, int canvaswidth)
        {
            _canvasLayers = LoadCanvasLayers(canvasheight, canvaswidth);
        }
        

        public static Canvas Create(List<int[][]> layers)
        {
            var canvas = new Canvas(layers[0].Count(), layers[0][0].Count());
            for (int i = 0; i < layers.Count; i++)
            {
                var canvasindex = canvas.CanvasLayers.ToList().FindIndex(cl => cl.Index == i);
                canvas.CanvasLayers[canvasindex].InitalizeLayer(layers[i]);
                
            }
            canvas.RefreshCanvas();
            return canvas;
        }

        private CanvasLayer _selected;

        public CanvasLayer Selected
        {
            get { return _selected; }
            set
            {
                _selected = value; 
                OnPropertyChanged("Selected");
            }
        }

    


        private ObservableCollection<CanvasLayer> _canvasLayers = new ObservableCollection<CanvasLayer>();

        public ObservableCollection<CanvasLayer> CanvasLayers
        {
            get { return _canvasLayers; }
        }

        private ObservableCollection<CanvasLayer> LoadCanvasLayers(int canvasheight, int canvaswidth)
        {
            var layers = new ObservableCollection<CanvasLayer>()
            {
                new CanvasLayer(0, canvasheight, canvaswidth), //middle
                new CanvasLayer(1, canvasheight, canvaswidth), //foreground
                new CanvasLayer(2, canvasheight, canvaswidth), //background
                new CanvasLayer(3, canvasheight, canvaswidth), //backdrop
            };
            return layers;
        }

        private void RefreshCanvas()
        {
            OnPropertyChanged("CanvasLayers");
        }
    }

}
