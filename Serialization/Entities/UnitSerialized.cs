using Newtonsoft.Json;
using Novalia.Entities;
using Novalia.Maps;
using SadRogue.Primitives;
using System;
using System.Runtime.Serialization;

namespace Novalia.Serialization.Entities
{
    public class UnitJsonConverter : JsonConverter<Unit>
    {
        public override void WriteJson(JsonWriter writer, Unit value, JsonSerializer serializer) => serializer.Serialize(writer, (UnitSerialized)value);

        public override Unit ReadJson(JsonReader reader, Type objectType, Unit existingValue,
                                        bool hasExistingValue, JsonSerializer serializer) => serializer.Deserialize<UnitSerialized>(reader);
    }

    [DataContract]
    public class UnitSerialized
    {
        [DataMember] public Point Position;
        [DataMember] string TemplateId;
        [DataMember] Guid UnitId;
        [DataMember] Guid EmpireId;
        [DataMember] Color EmpireColor;
        [DataMember] float Movement;
        [DataMember] float RemainingMovement;
        [DataMember] int MaxHealth;
        [DataMember] float RemainingHealth;
        [DataMember] int Strength;

        public static implicit operator UnitSerialized(Unit unit)
        {
            return new UnitSerialized()
            {
                Position = unit.Position,
                TemplateId = unit.TemplateId,
                UnitId = unit.Id,
                EmpireId = unit.EmpireId,
                EmpireColor = unit.EmpireColor,
                Movement = unit.Movement,
                RemainingMovement = unit.RemainingMovement,
                MaxHealth = unit.MaxHealth,
                RemainingHealth = unit.RemainingHealth,
                Strength = unit.Strength,
            };
        }

        public static implicit operator Unit(UnitSerialized serialized)
        {
            var template = UnitAtlas.ById[serialized.TemplateId];
            return new Unit(
                serialized.Position,
                template.Glyph,
                template.Name,
                false,
                true,
                (int)MapEntityLayer.ACTORS,
                serialized.UnitId,
                serialized.EmpireId,
                serialized.EmpireColor,
                serialized.TemplateId,
                serialized.Movement,
                serialized.MaxHealth,
                serialized.Strength)
            {
                RemainingMovement = serialized.RemainingMovement,
                RemainingHealth = serialized.RemainingHealth,
            };
        }
    }
}
