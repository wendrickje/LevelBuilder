using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using LevelBuilder.Common;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;

namespace LevelBuilder.ViewModel
{
    public class LevelConfiguration : Core
    {


        public LevelConfiguration(string name, int tilesize, string tilesheet)
        {
            Name = name;
            TileSize = tilesize;
            TileSheetResource = tilesheet;

            Palette.Update(tilesize, tilesheet, out _statusMessage);
            OnPropertyChanged("StatusMessage");
        }

        public int TileSize { get; private set; }

        public string Name { get; private set; }

        public string TileSheetResource { get; private set; }

        public void Rename(string newname)
        {
            Name = newname;
            OnPropertyChanged("Name");
        }

        #region apply changes

        private ICommand _applyChangesCommand;

        public ICommand ApplyChangesCommand
        {
            get { return _applyChangesCommand ?? (_applyChangesCommand = new RelayCommand(DoApplyChanges)); }
        }

        private void DoApplyChanges(object param)
        {
            StatusMessage = null;
            Palette.Update(TileSize, TileSheetResource, out _statusMessage);
            OnPropertyChanged("StatusMessage");

        }

        #endregion

        private string _statusMessage;

        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                OnPropertyChanged("StatusMessage");
            }
        }


        private Palette _palette;

        public Palette Palette
        {
            get { return _palette ?? (_palette = new Palette()); }
        }
        




    }
}
