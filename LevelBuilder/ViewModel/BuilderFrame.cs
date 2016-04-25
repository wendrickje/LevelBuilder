using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LevelBuilder.Common;
using Microsoft.Win32;
using System.IO;
using LevelBuilder.Source;
using System.Xml;
using System.IO.Packaging;
using System.IO.Compression;
using System.Diagnostics;

namespace LevelBuilder.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class BuilderFrame : Core
    {
        #region newCommand
        private ICommand _newCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand NewCommand
        {
            get { return _newCommand ?? (_newCommand = new RelayCommand(DoCreateNew, CanCreateNew)); }
        }

        private void DoCreateNew(object param)
        {
            //first save if there is an active project
            if (CanSave(null)) DoSave(null);
            //start fresh

            var newprojectwindow = new View.NewProjectWindow();
            App.Current.Resources.MergedDictionaries.ToList().ForEach(newprojectwindow.Resources.MergedDictionaries.Add);
            var result = newprojectwindow.ShowDialog();
            if (!result.HasValue || !result.Value) return;

            var tilesize = newprojectwindow.TileSize;
            var canvasheight = newprojectwindow.CanvasHeight;
            var canvaswidth = newprojectwindow.CanvasWidth;

            _project = new Project(newprojectwindow.ProjectName, canvasheight / tilesize, canvaswidth / tilesize, tilesize, newprojectwindow.TileSheetResource);
            OnPropertyChanged("Project");
            newprojectwindow = null;

        }

        private bool CanCreateNew(object param)
        {
            return true;
        }
        #endregion

        #region SaveCommand
        private ICommand _saveCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand SaveCommand
        {
            get { return _saveCommand ?? (_saveCommand = new RelayCommand(DoSave, CanSave)); }
        }

        private void DoSave(object param)
        {
            //save level manager (.lev.config) file
            //update active project
            Save(Project.LevelConfiguration.Name);
          
           
            

        }

        private bool CanSave(object param)
        {
            return Project != null;
        }
        #endregion
        
        #region SaveasCommand
        private ICommand _saveAsCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand SaveAsCommand
        {
            get { return _saveAsCommand ?? (_saveAsCommand = new RelayCommand(DoSaveAs, CanSaveAs)); }
        }

        private void DoSaveAs(object param)
        {
            //save new project 
            var savedialog = new SaveFileDialog();
            savedialog.ShowDialog();
            var dialogresult = savedialog.ShowDialog();
            if (!dialogresult.HasValue || !dialogresult.Value) return;
            Project.LevelConfiguration.Rename(savedialog.SafeFileName);
            Save(savedialog.SafeFileName);

        }

        private bool CanSaveAs(object param)
        {
            return Project != null;
        }

        private void Save(string name)
        {
           
          


            //var levelconfigfile = String.Format("{0}\\{1}{2}", GetProjectDirectory().FullName, name, Constants.levelconfigurationextension);
            //var levelconfigstream = new FileStream(levelconfigfile, FileMode.OpenOrCreate);
            //var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(LevelManager));
            //serializer.WriteObject(levelconfigstream, levelmanager);
            //levelconfigstream.Close();



            //save the level layout  (.lev) file
            var levellayoutfile = String.Format("{0}\\{1}{2}", GetProjectDirectory().FullName, name, Constants.levellayoutextension);
            var levellayoutstream = new StreamWriter(File.Create(levellayoutfile));

            foreach(var layer in Project.LevelCanvas.CanvasLayers)
            {
                levellayoutstream.WriteLine(String.Format("== layer {0} ==", layer.Index));
                var canvasrows = layer.CanvasTiles;
                
                foreach (var row in canvasrows)
                {
                    var cells = row;
                    var builder = new StringBuilder();
                    cells.ToList().ForEach(cell => builder.AppendFormat("{0},", cell));
                    var line = builder.ToString().TrimEnd(new char[]{','});
                    levellayoutstream.WriteLine(line);
                
                }

            }

            levellayoutstream.Close();

            var proj = new LevelBuilderProject.Project()
            {
                Name = name,
                CanvasHeight = Project.LevelCanvas.CanvasLayers[0].Height,
                CanvasWidth = Project.LevelCanvas.CanvasLayers[0].Width,
                TileSheet = Project.LevelConfiguration.TileSheetResource,
                TileSize = Project.LevelConfiguration.TileSize,
                Canvas = new LevelBuilderProject.Canvas()
                    {
                        Layers = Project.LevelCanvas.CanvasLayers.Select(lyr =>
                        {
                            var layer = new LevelBuilderProject.CanvasLayer() {Index = lyr.Index};
                            layer.Tiles = lyr.CanvasTiles.Select(tiles => tiles.ToArray()).ToArray();
                            return layer;

                        }).ToList()
                    }
            };
            var seralizer = new DataContractSerializer(typeof (LevelBuilderProject.Project));
            var projectfile = String.Format("{0}\\{1}{2}", GetProjectDirectory().FullName, name, Constants.projectFileExtension);
            var projectfilestream = (File.Create(projectfile));
            seralizer.WriteObject(projectfilestream, proj);
            projectfilestream.Close();
        }

        #endregion
        #region open
        private ICommand _openCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand OpenCommand
        {
            get { return _openCommand ?? (_openCommand = new RelayCommand(DoOpen, CanOpen)); }
        }

        private void DoOpen(object param)
        {
            //first save if there is an active project
            if(CanSave(null)) DoSave(null);
            var open = new OpenFileDialog()
            {
                Filter = String.Format("{0} (*{1})|*{1}", Constants.projectFileTypeName, Constants.projectFileExtension),
                DefaultExt = Constants.projectFileExtension,
                Multiselect = false,
            };
            var dialogresult = open.ShowDialog();
            if (!dialogresult.HasValue || !dialogresult.Value) return;
            var projfile = new FileInfo(open.FileName);
            if (projfile.Extension != Constants.projectFileExtension) return;

            IsLoading = true;
            using (var worker = new BackgroundWorker())
            {
                worker.DoWork += ((sender, args) =>
                {
                    var seralizer = new DataContractSerializer(typeof (LevelBuilderProject.Project));
                    var stream = File.OpenRead(projfile.FullName);
                    var proj = seralizer.ReadObject(stream) as LevelBuilderProject.Project;
                    if (proj == null)
                    {
                        args.Result = null;
                        return;
                    }
                    var canvas = proj.Canvas.Layers.Select(l => l.Tiles);
                    args.Result = new object[] {proj, canvas.ToList()};
                });
                worker.RunWorkerCompleted += ((sender, args) =>
                {
                    IsLoading = false;
                    if (args.Error != null)
                    {
                        Utilities.ExceptionUtilities.HandleException(args.Error);
                        return;
                    }
                    var result = args.Result as object[];
                    if (result == null)
                    {
                        Utilities.ExceptionUtilities.HandleException(new Exception(ErrorMessages.nullprojectfromfile));
                        return;
                    }
                    var proj = result[0] as LevelBuilderProject.Project;
                    var canvas = result[1] as List<int[][]>;
                    _project = new Project(proj.Name, proj.TileSize, proj.TileSheet, canvas);
                    
                    OnPropertyChanged("Project");

                });
                worker.RunWorkerAsync();
                
            }

        }

        private bool CanOpen(object param)
        {
            return true;
        }
        #endregion

        #region ExportCommand
        private ICommand _exportCommand;
        /// <summary>
        /// 
        /// </summary>
        public ICommand ExportCommand
        {
            get { return _exportCommand ?? (_exportCommand = new RelayCommand(DoExport, CanExport)); }
        }

        private void DoExport(object param)
        {
            //first save if there is an active project
            if(CanSave(null)) DoSave(null);

            
            var savedialog = new SaveFileDialog()
                {
                    FileName = Project.LevelConfiguration.Name,
                    OverwritePrompt = true,
                    AddExtension = true,
                    Title = Constants.exportDialogTitle,
                    DefaultExt = Constants.exportedExtension,
                    Filter = String.Format("{0} (*{1})|*{1}", Constants.exportedFileType, Constants.exportedExtension)

                };

            var dialog = savedialog.ShowDialog();
            if (!dialog.HasValue || !dialog.Value) return;
            var packagefile = new FileInfo(savedialog.FileName);
            var targetname = packagefile.FullName;
            

            var name = Project.LevelConfiguration.Name;
            //the level layout is the project file
            var projfileinfo = new FileInfo(String.Format("{0}\\{1}{2}", GetProjectDirectory().FullName, name, Constants.projectFileExtension));
            var levelconfigfile = projfileinfo.FullName.Replace(projfileinfo.Extension,
                Constants.levelconfigurationextension);
            projfileinfo.CopyTo(levelconfigfile, true);

            var levellayoutfile = String.Format("{0}\\{1}{2}", GetProjectDirectory().FullName, name, Constants.levellayoutextension);
            var tilesheetfile = Project.LevelConfiguration.TileSheetResource;
            

            //start export
            try
            {
                var sourcefiles = new List<FileInfo>() { new FileInfo(levelconfigfile), new FileInfo(levellayoutfile), new FileInfo(tilesheetfile) };
                var alllines = new List<byte[]>();
                using (FileStream fileToCompress = File.Create(targetname))
                {

                    foreach (var source in sourcefiles)
                    {
                        alllines.Add(System.Text.Encoding.Default.GetBytes("\r\n==" + source.Extension + "==\r\n"));
                        var bytesToCompress = File.ReadAllBytes(source.FullName);
                        alllines.Add(bytesToCompress);

                    }


                    using (DeflateStream compressionStream = new DeflateStream(fileToCompress, CompressionMode.Compress, true))
                    {
                        //var writer = new StreamWriter(compressionStream);
                        //writer.Write(alllines);
                        var bytearray = alllines.SelectMany(line => line).ToArray();
                        compressionStream.Write(bytearray, 0, bytearray.Length);
                    }


                }

                ////test decompress
                //var target = new FileInfo(targetname);

                //using (FileStream fileToDecompress = File.Open(target.FullName, FileMode.Open))
                //{
                //    using (DeflateStream decompressionStream = new DeflateStream(fileToDecompress, CompressionMode.Decompress))
                //    {
                //        var streamreader = new StreamReader(decompressionStream);
                //        while (!streamreader.EndOfStream)
                //        {
                //            var line = streamreader.ReadLine().Trim(null);
                //            Debug.WriteLine("decompressed: " + line);
                //        }
                //    }
                //}


                

                
            }
            catch(Exception e)
            {
                LevelBuilder.Common.Utilities.ExceptionUtilities.HandleException(e);
            }
            
        }

        private bool CanExport(object param)
        {
            return _project != null;
        }
        #endregion
        
        #region import
        private ICommand _importCommand;

        public ICommand ImportCommand
        {
            get {  return _importCommand ?? (_importCommand = new RelayCommand(DoImport));}
        }

        void DoImport(object param)
        {
            //first save if there is an active project
            if(CanSave(null)) DoSave(null);
            var open = new OpenFileDialog()
            {
                Filter = String.Format("{0} (*{1})|*{1}", Constants.exportedFileType, Constants.exportedExtension),
                Multiselect = false,
            };
            var dialogresult = open.ShowDialog();
            if (!dialogresult.HasValue || !dialogresult.Value) return;

            var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(LevelManager));

            //LevelManager manager;
            var target = new FileInfo(open.FileName);

            using (FileStream originalFileStream = target.OpenRead())
            {
                string currentFileName = target.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - target.Extension.Length);

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (DeflateStream decompressionStream = new DeflateStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", target.Name);
                    }
                }
            }

            //var sourcestream = new FileStream(targetname, FileMode.Open);
            //using (var decompressedStream = new MemoryStream())
            //{

            //    using (DeflateStream decompressionStream = new DeflateStream(sourcestream, CompressionMode.Decompress))
            //    {
            //        decompressionStream.CopyTo(decompressedStream);
            //    }

            //    manager = serializer.ReadObject(decompressedStream) as LevelManager;
            //}
            //sourcestream.Close();



        }
        #endregion
        #region close
        private ICommand _closeprojectCommand;

        public ICommand CloseProjectCommand
        {
            get { return _closeprojectCommand ?? (_closeprojectCommand= new RelayCommand(DoCloseProject, CanCloseProject)); }
        }

        private void DoCloseProject(object param)
        {
            //first save if there is an active project
            if(CanSave(null)) DoSave(null);

            _project = null;
            Mapper.Mappings.Clear();
            OnPropertyChanged("Project");

        }

        private bool CanCloseProject(object param)
        {
            return _project != null;
        }
        #endregion

        #region exit
        
        private ICommand _exitCommand;

        public ICommand ExitCommand
        {
            get { return _exitCommand ?? (_exitCommand = new RelayCommand(DoExit)); }
        }

        private void DoExit(object param)
        {
            //first save if there is an active project
            if(CanSave(null)) DoSave(null);

            Application.Current.Shutdown();

        }

        #endregion
        private Project _project;

        public Project Project
        {
            get { return _project; }
        }

        #region level manager helpers

        /// <summary> 
        ///   Copies data from a source stream to a target stream.</summary> 
        /// <param name="source">
        ///   The source stream to copy from.</param> 
        /// <param name="target">
        ///   The destination stream to copy to.</param> 
        private static void CopyStream(Stream source, Stream target)
        {
            const int bufSize = 0x1000;
            byte[] buf = new byte[bufSize];
            int bytesRead = 0;
            while ((bytesRead = source.Read(buf, 0, bufSize)) > 0)
                target.Write(buf, 0, bytesRead);
        }



        DirectoryInfo GetProjectDirectory()
        {
            var projectpath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, Constants.WorkingProjectDirectoryName, Project.LevelConfiguration.Name);
            var projectdirectory = new DirectoryInfo(projectpath);
            if(!projectdirectory.Exists) projectdirectory.Create();
            return projectdirectory;
        }
   
        #endregion

    }
}