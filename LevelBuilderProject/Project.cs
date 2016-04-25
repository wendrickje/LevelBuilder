using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LevelBuilderProject
{
    [DataContract]
    public class Project
    {
        [DataMember]
        public string Name { get; set; }
        
        [DataMember]
        public string TileSheet { get; set; }
        
        [DataMember]
        public int TileSize { get; set; }
        
        [DataMember]
        public int CanvasHeight { get; set; }

        [DataMember]
        public int CanvasWidth { get; set; }
        
        [DataMember]
        public Canvas Canvas { get; set; }
        
        [DataMember]
        public TileMap Mappings { get; set; }
    }
}
