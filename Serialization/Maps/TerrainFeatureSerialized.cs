using Newtonsoft.Json;
using Novalia.Entities;
using Novalia.Maps;
using SadRogue.Primitives;
using System.Runtime.Serialization;

namespace Novalia.Serialization.Maps
{
    public class TerrainFeatureJsonConverter : JsonConverter<Terrain>
    {
        public override void WriteJson(JsonWriter writer, Terrain value, JsonSerializer serializer) => serializer.Serialize(writer, (TerrainSerialized)value);

        public override Terrain ReadJson(JsonReader reader, System.Type objectType, Terrain existingValue,
                                        bool hasExistingValue, JsonSerializer serializer) => serializer.Deserialize<TerrainSerialized>(reader);
    }

    [DataContract]
    public class TerrainFeatureSerialized
    {
        [DataMember] public Point Position;
        [DataMember] public int Glyph;
        [DataMember] public string Name;
        [DataMember] public bool Transparent;

        public static implicit operator TerrainFeatureSerialized(Terrain terrain)
        {
            return new TerrainFeatureSerialized()
            {
                Position = terrain.Position,
                Glyph = terrain.Glyph,
                Name = terrain.Name,
                Transparent = terrain.IsTransparent,
            };
        }

        public static implicit operator TerrainFeature(TerrainFeatureSerialized serialized)
        {
            return new TerrainFeature(
                serialized.Position,
                serialized.Glyph,
                serialized.Name,
                serialized.Transparent);
        }
    }
}
