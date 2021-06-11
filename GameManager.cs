using GoRogue.MapGeneration;
using Novalia.Entities;
using Novalia.Fonts;
using Novalia.Logging;
using Novalia.Maps;
using Novalia.Ui;
using SadConsole;
using SadRogue.Integration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using Troschuetz.Random.Generators;

namespace Novalia
{
    internal sealed class GameManager : IGameManager
    {
        private readonly IUiManager _uiManager;
        private readonly IEntityFactory _entityFactory;
        private readonly ILogger _logger;

        public GameManager(
            IUiManager uiManager,
            IEntityFactory entityFactory,
            ILogger logger)
        {
            _uiManager = uiManager;
            _entityFactory = entityFactory;
            _logger = logger;
        }

        public bool CanLoad()
        {
            return false;
        }

        public void Load()
        {
            throw new System.NotImplementedException();
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        public void StartNewGame()
        {
            _logger.Debug("Starting new game.");
            var tilesetFont = Game.Instance.Fonts[_uiManager.TileFontName];
            var defaultFont = Game.Instance.DefaultFont;

            var tileSizeXFactor = (double)tilesetFont.GlyphWidth / defaultFont.GlyphWidth;
            var tileSizeYFactor = (double)tilesetFont.GlyphHeight / defaultFont.GlyphHeight;

            var tileWidth = (int)(_uiManager.ViewPortWidth / tileSizeXFactor);
            var tileHeight = (int)(_uiManager.ViewPortHeight / tileSizeYFactor);
            var generator = new Generator(tileWidth, tileHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(DefaultAlgorithms.RectangleMapSteps());
                });

            var generatedMap = generator.Context.GetFirst<ISettableGridView<bool>>("WallFloor");

            var map = new WorldMap(tileWidth, tileHeight, tilesetFont);

            foreach (var location in map.Positions())
            {
                bool walkable = generatedMap[location];
                int glyph = walkable ? WorldGlyphAtlas.Terrain_Grassland : WorldGlyphAtlas.Terrain_MapEdge;
                map.SetTerrain(new RogueLikeCell(location, Color.White, Color.Black, glyph, 0, walkable, walkable));
            }

            var rng = new StandardGenerator();
            for (int i = 0; i < 50; i++)
            {
                var position = map.WalkabilityView.RandomPosition(true, rng);
                var unit = _entityFactory.CreateUnit(position, UnitAtlas.CaveTroll, Color.PaleVioletRed);
                map.AddEntity(unit);
                position = map.WalkabilityView.RandomPosition(true, rng);
                unit = _entityFactory.CreateUnit(position, UnitAtlas.CaveTroll, Color.CornflowerBlue);
                map.AddEntity(unit);
            }

            Game.Instance.Screen = _uiManager.CreateMapScreen(map);
        }
    }
}
