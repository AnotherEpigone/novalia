﻿using Novalia.Maps;
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
                template.MaxHealth,
                template.Strength);

            return unit;
        }

        public NovaEntity CreateCity(Point position, string name, CityTemplate template, Guid empireId, Color empireColor)
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
    }
}
