using Novalia.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Novalia.Entities
{
    public static class TerrainFeatureAtlas
    {
        private static readonly Lazy<Dictionary<string, TerrainFeatureTemplate>> _byId;

        static TerrainFeatureAtlas()
        {
            _byId = new Lazy<Dictionary<string, TerrainFeatureTemplate>>(() => typeof(TerrainFeatureAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(TerrainFeatureTemplate))
                .Select(p => p.GetValue(null))
                .OfType<TerrainFeatureTemplate>()
                .ToDictionary(
                i => i.Id,
                i => i));
        }

        public static Dictionary<string, TerrainFeatureTemplate> ById => _byId.Value;

        public static TerrainFeatureTemplate Forest => new TerrainFeatureTemplate(
            "FEATURE_FOREST",
            WorldGlyphAtlas.Feature_Forest,
            "Forest",
            true);
    }
}
