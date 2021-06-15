using SadRogue.Primitives;
using System;

namespace Novalia.Entities
{
    public interface IEntityFactory
    {
        NovaEntity CreateUnit(Point position, UnitTemplate template, Guid empireId, Color factionColor);
    }
}