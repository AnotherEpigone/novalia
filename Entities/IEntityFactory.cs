using SadRogue.Primitives;
using System;

namespace Novalia.Entities
{
    public interface IEntityFactory
    {
        NovaEntity CreateCity(Point position, string name, CityTemplate template, Guid empireId, Color empireColor);
        NovaEntity CreateUnit(Point position, UnitTemplate template, Guid empireId, Color factionColor);
    }
}