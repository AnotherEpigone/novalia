using Newtonsoft.Json;
using Novalia.Entities;
using Novalia.Maps;
using SadRogue.Primitives;
using System.Runtime.Serialization;

namespace Novalia.Serialization.Maps
{
    public class TerrainFeatureJsonConverter : JsonConverter<TerrainFeature>
    {
        public override void WriteJson(JsonWriter writer, TerrainFeature value, JsonSerializer serializer) => serializer.Serialize(writer, (TerrainFeatureSerialized)value);

        public override TerrainFeature ReadJson(JsonReader reader, System.Type objectType, TerrainFeature existingValue,
                                        bool hasExistingValue, JsonSerializer serializer) => serializer.Deserialize<TerrainFeatureSerialized>(reader);
    }

    [DataContract]
    public class TerrainFeatureSerialized
    {
        [DataMember] public Point Position;
        [DataMember] public int Glyph;
        [DataMember] public string Name;
        [DataMember] public bool Transparent;
        [DataMember] public int MovementCost;

        public static implicit operator TerrainFeatureSerialized(TerrainFeature terrain)
        {
            return new TerrainFeatureSerialized()
            {
                Position = terrain.Position,
                Glyph = terrain.Glyph,
                Name = terrain.Name,
                Transparent = terrain.IsTransparent,
                MovementCost = terrain.MovementCost,
            };
        }

        public static implicit operator TerrainFeature(TerrainFeatureSerialized serialized)
        {
            return new TerrainFeature(
                serialized.Position,
                serialized.Glyph,
                serialized.Name,
                serialized.Transparent,
                serialized.MovementCost);
        }
    }
}
