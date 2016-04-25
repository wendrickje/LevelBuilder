using LevelBuilder.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelBuilder.ViewModel
{

    public class CanvasLayer : Core
    {
        public CanvasLayer(int index, int height, int width)
        {
            Width = width;
            Height = height;
            Index = index;
        }
        
        public void InitalizeLayer(int[][] tiles)
        {
           
            PopulateCanvas(tiles);
          
        }

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Index { get; private set; }


        

        private ObservableCollection<ObservableCollection<int>> _canvasTiles;

        public ObservableCollection<ObservableCollection<int>> CanvasTiles
        {
            get
            {
                if (_canvasTiles == null)
                {
                    _canvasTiles = new ObservableCollection<ObservableCollection<int>>();
                    InitalizeCanvas();
                }
                return _canvasTiles;
            }
        }
        private int _progress;

        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged("Progress");
            }
        }

        private void PopulateCanvas(int[][] tiles)
        {
            
            IsLoading = true;
            _canvasTiles = new ObservableCollection<ObservableCollection<int>>();
            var worker = new BackgroundWorker();
            worker.DoWork += ((sender, args) =>
                {
                        
                    //rows could be empty, need to handle this case
                    if (!tiles.Any())
                    {
                        
                        for (int y = 0; y < Height; y++)
                        {
                            var row = new ObservableCollection<int>();
                            for (int x = 0; x < Width; x++ )
                            {
                                row.Add(Common.ResourceKeys.Global.EmptyTileKey);
                            }
                            worker.ReportProgress(1, row);
                        }
                    }
                    else
                    {
                        foreach(var tilerow in tiles)
                        {
                            var row = new ObservableCollection<int>();
                            foreach (var tile in tilerow)
                            {
                                row.Add(tile);
                            }

                            worker.ReportProgress(1, row);
                        }
                    }

                        
                });
            worker.ProgressChanged += ((sender, args) =>
                {
                    var progrss = Progress + args.ProgressPercentage;
                    var row = args.UserState as ObservableCollection<int>;
                    CanvasTiles.Add(row);
                });
            worker.RunWorkerCompleted += ((sender, args) =>
                {
                    if(args.Error != null)
                    {
                        LevelBuilder.Common.Utilities.ExceptionUtilities.HandleException(args.Error);
                    }
                    worker.Dispose();
                    IsLoading = false;
                });
            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync();

        }

        void InitalizeCanvas()
        {
            IsLoading = true;
            _canvasTiles.Clear();
            var worker = new BackgroundWorker();
            worker.DoWork += ((sender, args) =>
                {
                        
                    
                    
                    for (int y = 0; y < Height; y++)
                    {
                        var row = new ObservableCollection<int>();
                        for (int x = 0; x < Width; x++ )
                        {
                            row.Add(Common.ResourceKeys.Global.EmptyTileKey);
                        }
                        worker.ReportProgress(1, row);
                    }

                        
                });
            worker.ProgressChanged += ((sender, args) =>
                {
                    var progrss = Progress + args.ProgressPercentage;
                    var row = args.UserState as ObservableCollection<int>;
                    CanvasTiles.Add(row);
                });
            worker.RunWorkerCompleted += ((sender, args) =>
                {
                    if(args.Error != null)
                    {
                        LevelBuilder.Common.Utilities.ExceptionUtilities.HandleException(args.Error);
                    }
                    worker.Dispose();
                    IsLoading = false;
                });
            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync();


        }
    }
}



