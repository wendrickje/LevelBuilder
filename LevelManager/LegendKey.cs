using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LevelManager
{
    [DataContract]
    public class LegendKey
    {
        /// <summary>
        /// the character in the .lev file
        /// </summary>
        [DataMember(Order = 0)]
        public string Id { get; set; }

        /// <summary>
        /// the tile the id represents
        /// </summary>
        [DataMember(Order = 1)]
        public string Value { get; set; }
    }
}
