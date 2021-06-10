using Novalia.Maps;
using SadConsole;
using SadRogue.Primitives;
using System;

namespace Novalia.Entities
{
    public class EntityFactory : IEntityFactory
    {
        public NovaEntity CreateUnit(Point position, UnitTemplate template, Color factionColor)
        {
            var unit = new NovaEntity(
                position,
                template.Glyph,
                template.Name,
                false,
                true,
                (int)MapEntityLayer.ACTORS,
                Guid.NewGuid());
            //unit.Appearance.Foreground = factionColor;

            return unit;
        }
    }
}
