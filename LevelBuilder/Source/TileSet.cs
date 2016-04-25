using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace LevelBuilder.Source
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

    }
}
