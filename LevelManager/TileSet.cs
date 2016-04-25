using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LevelManager
{
    [DataContract]
    public class TileSet
    {
        [DataMember(Order = 0)]
        public string Resource { get; set; }

        [DataMember(Order = 1)]
        public int TileSize { get; set; }

        [DataMember(Order = 2)]
        public int Rows { get; set; }

        [DataMember(Order = 3)]
        public int Columns { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [DataMember(Order = 4)]
        public string LevelLayout { get; set; }

    }
}
