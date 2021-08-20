using Newtonsoft.Json;
using Novalia.Maps;
using Novalia.Serialization.Entities;
using SadRogue.Primitives;
using System;
using System.Diagnostics;

namespace Novalia.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(CityJsonConverter))]
    public class City : NovaEntity
    {
        public City(
                Point position,
                int glyph,
                string name,
                Guid id,
                Guid empireId,
                Color empireColor,
                string templateId)
            : base(position, glyph, name, true, true, (int)MapEntityLayer.IMPROVEMENTS, id)
        {
            EmpireId = empireId;
            EmpireColor = empireColor;
            TemplateId = templateId;
        }

        public Guid EmpireId { get; }
        public Color EmpireColor { get; }
        public string TemplateId { get; }

        private string DebuggerDisplay => $"{nameof(City)}: {Name}";
    }
}
