using GoRogue.MapGeneration;
using Novalia.Maps.Generation.Steps;
using SadConsole;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using System;
using Troschuetz.Random;

namespace Novalia.Maps.Generation
{
    public class WorldMapFactory
    {
        public WorldMap Create(
            MapGenerationSettings settings,
            IFont tilesetFont,
            Point viewportSize,
            Guid playerEmpireId,
            IGenerator rng)
        {
            var iterations = (int)((settings.Width + settings.Height) * 1.5);
            GenerationStep continentsStep = settings.ContinentGeneratorStyle switch
            {
                ContinentGeneratorStyle.Continents => new ContinentsGenerationStep(rng, iterations, 0.3F),
                ContinentGeneratorStyle.Pangaea => new PangaeaGenerationStep(rng, iterations, 0.3F),
                _ => throw new ArgumentException("Unsupported continent generator style."),
            };

            var mapGenerator = new Generator(settings.Width, settings.Height)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(continentsStep);
                });

            var generatedMap = mapGenerator.Context.GetFirst<ISettableGridView<bool>>(((INovaGenerationStep)continentsStep).ComponentTag);

            var map = new WorldMap(settings.Width, settings.Height, tilesetFont, playerEmpireId);
            map.DefaultRenderer.Surface.View = map.DefaultRenderer.Surface.View.ChangeSize(
                viewportSize - map.DefaultRenderer.Surface.View.Size);

            foreach (var position in map.Positions())
            {
                var template = generatedMap[position] ? TerrainAtlas.Grassland : TerrainAtlas.Ocean;
                map.SetTerrain(new Terrain(position, template.Glyph, template.Name, template.Walkable, template.Transparent));
            }

            return map;
        }
    }
}
