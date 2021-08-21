using GoRogue.GameFramework;
using Newtonsoft.Json;
using Novalia.Fonts;
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
        private readonly NovaEntity _flag;

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

            _flag = new NovaEntity(position, WorldGlyphAtlas.UnitBanner, $"City banner {name}", true, true, (int)MapEntityLayer.EFFECTS, Guid.NewGuid());
            _flag.Appearance.Foreground = empireColor;

            AddedToMap += Unit_AddedToMap;
            RemovedFromMap += Unit_RemovedFromMap;
        }

        public Guid EmpireId { get; }
        public Color EmpireColor { get; }
        public string TemplateId { get; }

        private string DebuggerDisplay => $"{nameof(City)}: {Name}";

        private void HandleAddedToMap()
        {
            CurrentMap?.AddEntity(_flag);
        }

        private void HandleRemovedFromMap()
        {
            _flag.CurrentMap?.RemoveEntity(_flag);
        }

        private void Unit_RemovedFromMap(object sender, GameObjectCurrentMapChanged e) => HandleRemovedFromMap();

        private void Unit_AddedToMap(object sender, GameObjectCurrentMapChanged e) => HandleAddedToMap();
    }
}
