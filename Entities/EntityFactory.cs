using Novalia.Maps;
using SadRogue.Primitives;
using System;

namespace Novalia.Entities
{
    public class EntityFactory : IEntityFactory
    {
        public Unit CreateUnit(Point position, UnitTemplate template, Guid empireId, Color empireColor)
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
                template.MaxHealth,
                template.Strength);

            return unit;
        }

        public City CreateCity(Point position, string name, CityTemplate template, Guid empireId, Color empireColor)
        {
            var city = new City(
                position,
                template.Glyph,
                name,
                Guid.NewGuid(),
                empireId,
                empireColor,
                template.Id);

            return city;
        }

        public TerrainFeature CreateTerrainFeature(Point position, TerrainFeatureTemplate template)
        {
            var feature = new TerrainFeature(
                position,
                template.Glyph,
                template.Name,
                template.Transparent,
                template.MovementCost);
            return feature;
        }
    }
}
