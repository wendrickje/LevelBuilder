using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelBuilder.ViewModel
{
    public class Project : Core
    {
        public Project(string name, int canvasheight, int canvaswidth, int tilesize, string tilesheet)
        {
            _levelConfigurator = new LevelConfiguration(name, tilesize, tilesheet);
            _levelCanvas = new Canvas(canvasheight, canvaswidth);

        }
        
        public Project(string name, int tilesize, string tilesheet, List<int[][]> layers)
        {
            _levelConfigurator = new LevelConfiguration(name, tilesize, tilesheet);
            _levelCanvas = Canvas.Create(layers);

        }

        private LevelConfiguration _levelConfigurator;

        /// <summary>
        /// 
        /// </summary>
        public LevelConfiguration LevelConfiguration
        {
            get { return _levelConfigurator; }
        }

        private Canvas _levelCanvas;

        /// <summary>
        /// 
        /// </summary>
        public Canvas LevelCanvas
        {
            get { return _levelCanvas; }
        }
    }
}
