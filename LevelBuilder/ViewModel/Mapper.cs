using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelBuilder.ViewModel
{
    public class Mapper : Core
    {
        private Mapper()
        {

        }

        private static Mapper _instance;
        private Dictionary<int, PaletteItem> _mappings;

        public static Dictionary<int, PaletteItem> Mappings
        {
            get { return GetInstance()._mappings; }
        }

        private static Mapper GetInstance()
        {
            return Mapper._instance ?? (Mapper._instance = new Mapper() { _mappings = new Dictionary<int, PaletteItem>() });
        }
    }
}
