using System.Runtime.Serialization;

namespace LevelBuilder.Source
{
    [DataContract]
    public class TileMap
    {
        /// <summary>
        /// a unique key for the tile
        /// </summary>
        [DataMember(Order = 0)]
        public string Id { get; set; }

        /// <summary>
        ///the tile's y index in the tile sheet
        /// </summary>
        [DataMember(Order = 1)]
        public int? Row { get; set; }

        /// <summary>
        ///the tile's x index in the tile sheet
        /// </summary>
        [DataMember(Order = 2)]
        public int? Column { get; set; }

    }
}
