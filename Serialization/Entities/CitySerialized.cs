using Newtonsoft.Json;
using Novalia.Entities;
using SadRogue.Primitives;
using System;
using System.Runtime.Serialization;

namespace Novalia.Serialization.Entities
{
    public class CityJsonConverter : JsonConverter<City>
    {
        public override void WriteJson(JsonWriter writer, City value, JsonSerializer serializer) => serializer.Serialize(writer, (CitySerialized)value);

        public override City ReadJson(JsonReader reader, Type objectType, City existingValue,
                                        bool hasExistingValue, JsonSerializer serializer) => serializer.Deserialize<CitySerialized>(reader);
    }

    [DataContract]
    public class CitySerialized
    {
        [DataMember] public Point Position;
        [DataMember] string TemplateId;
        [DataMember] Guid CityId;
        [DataMember] Guid EmpireId;
        [DataMember] Color EmpireColor;
        [DataMember] string Name;

        public static implicit operator CitySerialized(City city)
        {
            return new CitySerialized()
            {
                Position = city.Position,
                TemplateId = city.TemplateId,
                CityId = city.Id,
                EmpireId = city.EmpireId,
                EmpireColor = city.EmpireColor,
            };
        }

        public static implicit operator City(CitySerialized serialized)
        {
            //var template = UnitAtlas.ById[serialized.TemplateId];
            return new City(
                serialized.Position,
                0, // TODO
                serialized.Name,
                serialized.CityId,
                serialized.EmpireId,
                serialized.EmpireColor,
                serialized.TemplateId);
        }
    }
}
