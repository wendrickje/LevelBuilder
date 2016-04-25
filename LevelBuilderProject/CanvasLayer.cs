using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LevelBuilderProject
{
    [DataContract]
    public class CanvasLayer
    {
        [DataMember]
        public int Index { get; set; }

        [DataMember]
        public int[][] Tiles { get; set; }
    }
}
