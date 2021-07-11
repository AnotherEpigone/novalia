using Newtonsoft.Json;
using Novalia.Entities;
using Novalia.Extensions;
using Novalia.Maps;
using SadConsole;
using System;
using System.Linq;
using System.Runtime.Serialization;

namespace Novalia.Serialization.Maps
{
    public class WorldMapJsonConverter : JsonConverter<WorldMap>
    {
        public override void WriteJson(JsonWriter writer, WorldMap value, JsonSerializer serializer) => serializer.Serialize(writer, (WorldMapSerialized)value);

        public override WorldMap ReadJson(JsonReader reader, System.Type objectType, WorldMap existingValue,
                                        bool hasExistingValue, JsonSerializer serializer) => serializer.Deserialize<WorldMapSerialized>(reader);
    }

    [DataContract]
    public class WorldMapSerialized
    {
        [DataMember] public int Width;
        [DataMember] public int Height;
        [DataMember] public string FontId;
        [DataMember] public Terrain[] Terrain;
        [DataMember] public Unit[] Units;
        [DataMember] Guid PlayerEmpireId;

        public static implicit operator WorldMapSerialized(WorldMap map)
        {
            return new WorldMapSerialized()
            {
                Width = map.Width,
                Height = map.Height,
                FontId = map.Font.Name,
                Terrain = map.Terrain.ToEnumerable().Cast<Terrain>().ToArray(),
                Units = map.Entities.Items.OfType<Unit>().ToArray(),
                PlayerEmpireId = map.PlayerEmpireId,
            };
        }

        public static implicit operator WorldMap(WorldMapSerialized serialized)
        {
            var map = new WorldMap(
                serialized.Width,
                serialized.Height,
                Game.Instance.Fonts[serialized.FontId],
                serialized.PlayerEmpireId);
            foreach (var terrain in serialized.Terrain)
            {
                map.SetTerrain(terrain);
            }
            
            foreach (var unit in serialized.Units)
            {
                map.AddEntity(unit);
            }

            return map;
        }
    }
}
