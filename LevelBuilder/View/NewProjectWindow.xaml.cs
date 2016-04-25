using LevelBuilder.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using Microsoft.Win32;

namespace LevelBuilder.View
{
    /// <summary>
    /// Interaction logic for NewProjectWindow.xaml
    /// </summary>
    public partial class NewProjectWindow : Window, IDataErrorInfo, INotifyPropertyChanged
    {
        public NewProjectWindow()
        {
            InitializeComponent();
        }

        private int _canvasHeight = 3200;

        public int CanvasHeight
        {
            get { return _canvasHeight; }
            set
            {
                _canvasHeight = value;
                OnPropertyChanged("CanvasHeight");
                OnPropertyChanged("CanvasPixelSize");
                OnPropertyChanged("CanvasTileSize");
            }
        }
        private int _canvasWidth = 3200;

        public int CanvasWidth
        {
            get { return _canvasWidth; }
            set
            {
                _canvasWidth = value;
                OnPropertyChanged("CanvasWidth");
                OnPropertyChanged("CanvasPixelSize");
                OnPropertyChanged("CanvasTileSize");
            }
        }

        public string CanvasPixelSize
        {
            get 
            {
                return String.Format("{0} x {1}", CanvasHeight, CanvasWidth);
            }
        }
        public string CanvasTileSize
        {
            get
            {
                return String.Format("{0} x {1}", CanvasHeight / TileSize, CanvasWidth / TileSize);
            }
        }

        private int _tileSize = 32;

        public int TileSize
        {
            get { return _tileSize; }
            set
            {
                _tileSize = value;
                OnPropertyChanged("TileSize");
            }
        }

        private string _projectname;

        public string ProjectName
        {
            get { return _projectname; }
            set
            {
                _projectname = value;
                OnPropertyChanged("ProjectName");
            }
        }

        private string _tilesheetresource;

        public string TileSheetResource
        {
            get { return _tilesheetresource; }
            set
            {
                _tilesheetresource = value;
                OnPropertyChanged("TileSheetResource");
            }
        }

        private string _filename;

        public string FileName
        {
            get { return _filename; }
            set
            {
                _filename = value;
                OnPropertyChanged("FileName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged(string prop)
        {
            var pchd = PropertyChanged;
            if (pchd == null) return;
            pchd(this, new PropertyChangedEventArgs(prop));
        }

        string _error;
        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }

        public string this[string columnName]
        {
            get
            {
                //todo: implement validation
                //tilesize
                //canvasheight
                //canvaswidth
                if (columnName == "ProjectName")
                    return ValidateName();
               

                return null;
            }
        }

        #region validation

        public string ValidateName()
        {
            var name = ProjectName;
            if (String.IsNullOrEmpty(name)) return ErrorMessages.newprojectwindow_nullname;

            var path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, Constants.WorkingProjectDirectoryName, name);
            return Directory.Exists(path) ? ErrorMessages.newprojectwindow_uniquename : null;
        }
        #endregion

        #region createcommand

        private ICommand _createCommand;

        public ICommand CreateCommand
        {
            get
            {
                return _createCommand ??
                       (_createCommand = new RelayCommand(DoCreateProject, CanCreateProject));
            }
        }
        
        private void DoCreateProject(object param)
        {

            var path = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, Constants.WorkingProjectDirectoryName, ProjectName);
            var directory = Directory.CreateDirectory(path);
            var existingfiles = directory.EnumerateFiles().Where(f =>
            {
                var filename = f.Name.Trim(f.Extension.ToCharArray());
                return filename == Constants.TileSheetName;
            });


            try
            {
                if (existingfiles.Any())
                {

                    existingfiles.ToList().ForEach(f =>
                    {
                        f.OpenRead().Close();
                        f.Delete();
                    });
                }
            }
            catch (Exception e)
            {
                LevelBuilder.Common.Utilities.ExceptionUtilities.HandleException(e);
                return;
            }
            var file = new FileInfo(FileName);
            var copied = file.CopyTo(Path.Combine(path, Common.Constants.TileSheetName) + file.Extension, true);



            TileSheetResource = copied.FullName;
            Error = null;
            //Palette.Update(TileSize, TileSheetResource, out _statusMessage);
            //OnPropertyChanged("StatusMessage");
            file = null;
            copied = null;

            this.DialogResult = true;
        }

        private bool CanCreateProject(object param)
        {
            return !String.IsNullOrEmpty(ProjectName)
                && !String.IsNullOrEmpty(FileName)
                && String.IsNullOrEmpty(Error);
        }
        #endregion


        #region BrowseForTileSheetCommand
        private ICommand _browseForTileSheetCommand;

        public ICommand BrowseForTileSheetCommand
        {
            get
            {
                return _browseForTileSheetCommand ??
                       (_browseForTileSheetCommand = new RelayCommand(DoBrowseForTileSheet, CanBrowse));
            }
        }
        private bool CanBrowse(object param)
        {
            return true;
        }
        private void DoBrowseForTileSheet(object param)
        {

            var open = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                AddExtension = true,
                Multiselect = false,
                ShowReadOnly = false,
                ValidateNames = true,
                Filter = "All Supported Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png;"
            };
            var dialog = open.ShowDialog();
            if (!dialog.HasValue || !dialog.Value) return;
            FileName = open.FileName;

        }


        #endregion
    }
}
