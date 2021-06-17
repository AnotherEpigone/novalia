using GoRogue.GameFramework;
using Novalia.Fonts;
using Novalia.Maps;
using SadRogue.Primitives;
using System;
using System.Diagnostics;

namespace Novalia.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Unit : NovaEntity
    {
        private readonly NovaEntity _flag;

        public Unit(
                Point position,
                int glyph,
                string name,
                bool walkable,
                bool transparent,
                int layer,
                Guid id,
                Guid empireId,
                Color empireColor)
            : base(position, glyph, name, walkable, transparent, layer, id)
        {
            EmpireId = empireId;

            _flag = new NovaEntity(position, WorldGlyphAtlas.UnitBanner, $"Unit banner {name}", true, true, (int)MapEntityLayer.EFFECTS, Guid.NewGuid());
            _flag.Appearance.Foreground = empireColor;
        }

        public Guid EmpireId { get; }

        private string DebuggerDisplay => $"{nameof(Unit)}: {Name}";

        public override void OnMapChanged(Map newMap)
        {
            base.OnMapChanged(newMap);

            _flag.CurrentMap?.RemoveEntity(_flag);
            newMap.AddEntity(_flag);
        }
    }
}
