using Novalia.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Novalia.Maps
{
    public static class TerrainAtlas
    {
        private static readonly Lazy<Dictionary<string, TerrainTemplate>> _byId;

        static TerrainAtlas()
        {
            _byId = new Lazy<Dictionary<string, TerrainTemplate>>(() => typeof(TerrainAtlas)
                .GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Where(p => p.PropertyType == typeof(TerrainTemplate))
                .Select(p => p.GetValue(null))
                .OfType<TerrainTemplate>()
                .ToDictionary(
                i => i.Id,
                i => i));
        }

        public static Dictionary<string, TerrainTemplate> ById => _byId.Value;

        public static TerrainTemplate MapEdge => new TerrainTemplate(
            "TERRAIN_MAPEDGE",
            "Map edge",
            WorldGlyphAtlas.Terrain_MapEdge,
            false,
            false);

        public static TerrainTemplate Grassland => new TerrainTemplate(
            "TERRAIN_GRASSLAND",
            "Grassland",
            WorldGlyphAtlas.Terrain_Grassland,
            true,
            true);
    }
}
