using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LevelManager
{
    [DataContract]
    public class TileMap
    {
        [DataMember(Order = 0)]
        public string Id { get; set; }

        [DataMember(Order = 1)]
        public int? Row { get; set; }

        [DataMember(Order = 2)]
        public int? Column { get; set; }

    }
}
