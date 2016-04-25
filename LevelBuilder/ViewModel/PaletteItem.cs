using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LevelBuilder.ViewModel
{
    public class PaletteItem : Core
    {

        public PaletteItem(int x, int y, int size, Bitmap image, int key)
        {
            XAxis = x;
            YAxis = y;
            Size = size;
            Bitmap = image;
            Key = key;
        }

        /// <summary>
        /// x pixel position on the tilesheet
        /// </summary>
        public int XAxis { get; private set; }
        /// <summary>
        /// y pixel position on the tilesheet
        /// </summary>
        public int YAxis { get; private set; }

        /// <summary>
        /// x index of tile on tilesheet
        /// </summary>
        public int XIndex { get { return XAxis / Size; } }

        /// <summary>
        /// y index of tile on tilesheet
        /// </summary>
        public int YIndex { get { return YAxis / Size; } }

        public int Size { get; private set; }
        public Bitmap Bitmap { get; private set; }

        public int Key { get; private set; }

        public ICommand Command { get; set; }

    }
}
