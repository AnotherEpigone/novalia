using Novalia.Maps;
using SadRogue.Primitives;
using System;

namespace Novalia.Entities
{
    public class EntityFactory : IEntityFactory
    {
        public NovaEntity CreateUnit(Point position, UnitTemplate template, Guid empireId, Color empireColor)
        {
            var unit = new Unit(
                position,
                template.Glyph,
                template.Name,
                false,
                true,
                (int)MapEntityLayer.ACTORS,
                Guid.NewGuid(),
                empireId,
                empireColor,
                template.Id,
                template.Movement,
                template.MaxHealth);

            return unit;
        }
    }
}
