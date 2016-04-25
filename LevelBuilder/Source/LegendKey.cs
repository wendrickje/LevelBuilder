using System.Runtime.Serialization;

namespace LevelBuilder.Source
{
    [DataContract]
    public class LegendKey
    {
        /// <summary>
        /// the int in the .lev file
        /// </summary>
        [DataMember(Order = 0)]
        public int Id { get; set; }

        /// <summary>
        /// the tile the id represents
        /// </summary>
        [DataMember(Order = 1)]
        public int TileMapId { get; set; }
    }
}
