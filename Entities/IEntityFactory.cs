using SadRogue.Primitives;
using System;

namespace Novalia.Entities
{
    public interface IEntityFactory
    {
        City CreateCity(Point position, string name, CityTemplate template, Guid empireId, Color empireColor);
        Unit CreateUnit(Point position, UnitTemplate template, Guid empireId, Color factionColor);
        TerrainFeature CreateTerrainFeature(Point position, TerrainFeatureTemplate template);
    }
}