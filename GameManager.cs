using GoRogue.MapGeneration;
using Novalia.Entities;
using Novalia.GameMechanics;
using Novalia.Logging;
using Novalia.Maps;
using Novalia.Ui;
using SadConsole;
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

        private WorldMap _map;

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

        public void LoadLatest()
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

            _map = new WorldMap(tileWidth, tileHeight, tilesetFont);

            foreach (var position in _map.Positions())
            {
                var template = generatedMap[position] ? TerrainAtlas.Grassland : TerrainAtlas.MapEdge;
                _map.SetTerrain(new Terrain(position, template.Glyph, template.Name, template.Walkable, template.Transparent));
            }

            // TODO: attach the concrete empires and map to a game object
            var sudet = new Empire(EmpireAtlas.Sudet);
            var blackhand = new Empire(EmpireAtlas.BlackhandDominion);

            var rng = new StandardGenerator();
            for (int i = 0; i < 50; i++)
            {
                var position = _map.WalkabilityView.RandomPosition(true, rng);
                var unit = _entityFactory.CreateUnit(position, UnitAtlas.CaveTroll, sudet.Id, Color.Red);
                _map.AddEntity(unit);

                position = _map.WalkabilityView.RandomPosition(true, rng);
                unit = _entityFactory.CreateUnit(position, UnitAtlas.CaveTroll, blackhand.Id, Color.Blue);
                _map.AddEntity(unit);
            }

            var game = new NovaGame(sudet.Id, new Empire[] { sudet, blackhand });

            Game.Instance.Screen = _uiManager.CreateMapScreen(this, _map, game);
        }
    }
}
