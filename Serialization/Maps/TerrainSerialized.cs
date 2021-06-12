using Newtonsoft.Json;
using Novalia.Maps;
using SadRogue.Primitives;
using System.Runtime.Serialization;

namespace Novalia.Serialization.Maps
{
    public class TerrainJsonConverter : JsonConverter<Terrain>
    {
        public override void WriteJson(JsonWriter writer, Terrain value, JsonSerializer serializer) => serializer.Serialize(writer, (TerrainSerialized)value);

        public override Terrain ReadJson(JsonReader reader, System.Type objectType, Terrain existingValue,
                                        bool hasExistingValue, JsonSerializer serializer) => serializer.Deserialize<TerrainSerialized>(reader);
    }

    [DataContract]
    public class TerrainSerialized
    {
        [DataMember] public Point Position;
        [DataMember] public int Glyph;
        [DataMember] public string Name;
        [DataMember] public bool Walkable;
        [DataMember] public bool Transparent;

        public static implicit operator TerrainSerialized(Terrain terrain)
        {
            return new TerrainSerialized()
            {
                Position = terrain.Position,
                Glyph = terrain.Glyph,
                Name = terrain.Name,
                Walkable = terrain.IsWalkable,
                Transparent = terrain.IsTransparent,
            };
        }

        public static implicit operator Terrain(TerrainSerialized serialized)
        {
            return new Terrain(
                serialized.Position,
                serialized.Glyph,
                serialized.Name,
                serialized.Walkable,
                serialized.Transparent);
        }
    }
}
