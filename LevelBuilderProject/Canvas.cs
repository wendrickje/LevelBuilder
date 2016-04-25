using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace LevelBuilderProject
{
    [DataContract]
    public class Canvas
    {

        [DataMember]
        public List<CanvasLayer> Layers { get; set; }

    }
}
