using Novalia.GameMechanics;
using Novalia.Maps;
using System;
using System.Runtime.Serialization;

namespace Novalia.Serialization
{
    [DataContract]
    public class GameState
    {
        [DataMember]
        public Guid PlayerEmpireId { get; set; }

        [DataMember]
        public Empire[] Empires { get; set; }

        [DataMember]
        public WorldMap Map { get; set; }
    }
}
