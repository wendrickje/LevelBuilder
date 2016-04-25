using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace LevelBuilder.Source
{
    [DataContract]
    public class LevelManager 
    {

        public LevelManager()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static LevelManager Create(string name)
        {
            var serializer = new System.Runtime.Serialization.DataContractSerializer(typeof(LevelManager));
            var file = String.Format("levels/layouts/{0}.lev.xml", name);
            var stream = new FileStream(file, FileMode.Open);

            var manager = serializer.ReadObject(stream) as LevelManager;

            return manager;
        }

        /// <summary>
        /// 
        /// </summary>
        public string LevelLayout { get { return String.Format("levels/layouts/{0}.lev", Name); } }


        [DataMember(Order = 0)]
        public string Name { get; set; }

        [DataMember(Order = 1)]
        public TileSet TileSet { get; set; }

        [DataMember(Order = 2)]
        public List<TileMap> TileMaps { get; set; }

        [DataMember(Order = 3)]
        public List<LegendKey> Legend { get; set; }

    }
}
