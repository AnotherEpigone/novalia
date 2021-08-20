using Novalia.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Novalia.Entities
{
    public static class CityAtlas
    {
        private static readonly Lazy<Dictionary<string, CityTemplate>> _byId;

        static CityAtlas()
        {
            _byId = new Lazy<Dictionary<string, CityTemplate>>(() => typeof(CityAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(CityTemplate))
                .Select(p => p.GetValue(null))
                .OfType<CityTemplate>()
                .ToDictionary(
                i => i.Id,
                i => i));
        }

        public static Dictionary<string, CityTemplate> ById => _byId.Value;

        public static CityTemplate HumanTown => new CityTemplate(
            "CITY_HUMAN",
            WorldGlyphAtlas.City);
    }
}
