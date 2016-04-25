using LevelBuilder.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LevelBuilder.ViewModel
{
    public class Palette : Core
    {
        
        public Palette()
        {
        }


        private ObservableCollection<PaletteItem> _selected;

        public ObservableCollection<PaletteItem> SelectedItems
        {
            get 
            {
                return _selected ?? (_selected = new ObservableCollection<PaletteItem>());
            }
            set
            {
                _selected = value;
                OnPropertyChanged("SelectedItems");
            }
        }

        private PaletteToolType _currentTool = PaletteToolType.Selector;

        public PaletteToolType CurrentTool
        {
            get { return _currentTool; }
            set
            {
                _currentTool = value;
                OnPropertyChanged("CurrentTool");
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


        private ObservableCollection<ObservableCollection<PaletteItem>> _paletteItems;

        public ObservableCollection<ObservableCollection<PaletteItem>> PaletteItems
        {
            get 
            {
                return _paletteItems ?? (_paletteItems = new ObservableCollection<ObservableCollection<PaletteItem>>());
            }
        }

        public void Update(int tilesize, string resource, out string status)
        {
            if (String.IsNullOrEmpty(resource) || tilesize <= 0 || !File.Exists(resource))
            {
                status = Common.Constants.TilePalette_EmptyWarning;
                return;
            }
            PaletteItems.Clear();
            Mapper.Mappings.Clear();
            IsLoading = true;
            var worker = new BackgroundWorker();
            
            var sheet = new Bitmap(resource);
            var rows = sheet.Height / tilesize;
            var columns = sheet.Width / tilesize;

            if (sheet.Height % tilesize != 0 || sheet.Width % tilesize != 0)
            {
                status = Common.Constants.TilePalette_ExceedsWarning;
            }
            else status = null;
            worker.DoWork += ((sender, args) =>
                {
                        
                    
                    //reserved:             1 =: start
                    //                      0 =: nothing/empty space
                    //                      2 =: event
                    int key = 3;


                    for (int y = 0; y < rows; y++)
                    {
                        var row = new List<PaletteItemTransferObject>();
                        for (int x = 0; x < columns; x++)
                        {


                            var cellBoundary = new Rectangle(x * tilesize, y * tilesize, tilesize, tilesize);
                            var cell = CropImage(sheet, cellBoundary);

                            var transfer = new PaletteItemTransferObject()
                            {
                                X = x * tilesize,
                                Y = y * tilesize,
                                Image = cell,
                                Key = key
                            };
                            row.Add(transfer);
                            cell = null;
                            transfer = null;
                            key++;
                        }
                        worker.ReportProgress(1, row);
                    }

                        
                });
            worker.ProgressChanged += ((sender, args) =>
                {
                    var progrss = Progress + args.ProgressPercentage;

                    var palrows = args.UserState as List<PaletteItemTransferObject>;
                    if (palrows == null) return;
                    PaletteItems.Add(new ObservableCollection<PaletteItem>());
                    foreach (var pal in palrows)
                    {
                        var paletteitem = new PaletteItem(pal.X, pal.Y, tilesize, pal.Image, pal.Key);
                        Mapper.Mappings.Add(pal.Key, paletteitem);
                        PaletteItems.Last().Add(paletteitem);
                    }
                    
                });
            worker.RunWorkerCompleted += ((sender, args) =>
                {
                    if(args.Error != null)
                    {
                        LevelBuilder.Common.Utilities.ExceptionUtilities.HandleException(args.Error);
                    }
                    sheet.Dispose();
                    worker.Dispose();
                    IsLoading = false;
                });
            worker.WorkerReportsProgress = true;
            worker.RunWorkerAsync();
            


            
        }

        /// <summary>
        /// crops image
        /// </summary>
        /// <param name="img">source image to crop</param>
        /// <param name="cropArea">rectangle used for cropping</param>
        /// <returns>new cropped image</returns>
        private static Bitmap CropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }
    }

    internal class PaletteItemTransferObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Bitmap Image { get; set; }
        public int Key { get; set; }
    }
}
