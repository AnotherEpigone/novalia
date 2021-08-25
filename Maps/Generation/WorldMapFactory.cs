using GoRogue.MapGeneration;
using Novalia.Entities;
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
            IGenerator rng,
            IEntityFactory entityFactory)
        {
            var tileCount = settings.Width * settings.Height;
            var landmassIterations = (int)((settings.Width + settings.Height) * 1.5);
            GenerationStep continentsStep = settings.ContinentGeneratorStyle switch
            {
                ContinentGeneratorStyle.Continents => new ContinentsGenerationStep(rng, landmassIterations, 0.3F),
                ContinentGeneratorStyle.Pangaea => new PangaeaGenerationStep(rng, landmassIterations, 0.3F),
                _ => throw new ArgumentException("Unsupported continent generator style."),
            };
            var forestStep = new LandFeatureGenerationStep("Forests", rng, tileCount / 5, tileCount / 2, (INovaGenerationStep)continentsStep);

            var mapGenerator = new Generator(settings.Width, settings.Height)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddStep(continentsStep);
                    gen.AddStep(forestStep);
                });

            var map = new WorldMap(settings.Width, settings.Height, tilesetFont);
            map.DefaultRenderer.Surface.View = map.DefaultRenderer.Surface.View.ChangeSize(
                viewportSize - map.DefaultRenderer.Surface.View.Size);

            var continentsMap = mapGenerator.Context.GetFirst<ISettableGridView<bool>>(((INovaGenerationStep)continentsStep).ComponentTag);
            var forestsMap = mapGenerator.Context.GetFirst<ISettableGridView<bool>>(((INovaGenerationStep)forestStep).ComponentTag);
            foreach (var position in map.Positions())
            {
                var template = continentsMap[position] ? TerrainAtlas.Grassland : TerrainAtlas.Ocean;
                map.SetTerrain(new Terrain(position, template.Glyph, template.Name, template.Walkable, template.Transparent));

                if (forestsMap[position])
                {
                    var forest = entityFactory.CreateTerrainFeature(position, TerrainFeatureAtlas.Forest);
                    map.AddEntity(forest);
                }
            }


            return map;
        }
    }
}
