using GoRogue.MapGeneration;
using Novalia.Entities;
using Novalia.GameMechanics;
using Novalia.Logging;
using Novalia.Maps;
using Novalia.Serialization;
using Novalia.Ui;
using Novalia.Ui.Consoles;
using SadConsole;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using System.Linq;
using Troschuetz.Random.Generators;

namespace Novalia
{
    internal sealed class GameManager : IGameManager
    {
        private readonly IUiManager _uiManager;
        private readonly IEntityFactory _entityFactory;
        private readonly ILogger _logger;
        private readonly ISaveManager _saveManager;

        public GameManager(
            IUiManager uiManager,
            IEntityFactory entityFactory,
            ILogger logger,
            ISaveManager saveManager)
        {
            _uiManager = uiManager;
            _entityFactory = entityFactory;
            _logger = logger;
            _saveManager = saveManager;
        }

        public bool CanLoad()
        {
            return _saveManager.SaveExists();
        }

        public void Load()
        {
            var (success, gameState) = _saveManager.Read();
            if (!success)
            {
                throw new System.IO.IOException("Failed to load save file.");
            }

            var game = new NovaGame(gameState.PlayerEmpireId, gameState.Empires);
            Game.Instance.Screen = _uiManager.CreateMapScreen(this, gameState.Map, game);
        }

        public void LoadLatest()
        {
            var (success, gameState) = _saveManager.Read();
            if (!success)
            {
                throw new System.IO.IOException("Failed to load save file.");
            }

            var game = new NovaGame(gameState.PlayerEmpireId, gameState.Empires);
            Game.Instance.Screen = _uiManager.CreateMapScreen(this, gameState.Map, game);
            Game.Instance.DestroyDefaultStartingConsole();
            Game.Instance.Screen.IsFocused = true;
        }

        public void Save()
        {
            if (Game.Instance.Screen is not MainConsole mainConsole)
            {
                return;
            }

            var save = new GameState()
            {
                Empires = mainConsole.Game.Empires.Values.ToArray(),
                PlayerEmpireId = mainConsole.Game.PlayerEmpireId,
                Map = mainConsole.Map,
            };

            _saveManager.Write(save);
        }

        public void StartNewGame()
        {
            _logger.Debug("Starting new game.");
            var tilesetFont = Game.Instance.Fonts[_uiManager.TileFontName];
            var defaultFont = Game.Instance.DefaultFont;

            var tileWidth = 80;
            var tileHeight = 80;
            var generator = new Generator(tileWidth, tileHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(DefaultAlgorithms.RectangleMapSteps());
                });

            var generatedMap = generator.Context.GetFirst<ISettableGridView<bool>>("WallFloor");

            var sudet = new Empire(EmpireAtlas.Sudet);
            var blackhand = new Empire(EmpireAtlas.BlackhandDominion);
            var playerEmpire = sudet;

            var map = new WorldMap(tileWidth, tileHeight, tilesetFont, playerEmpire.Id);
            map.DefaultRenderer.Surface.View = map.DefaultRenderer.Surface.View.ChangeSize(
                GetViewportSizeInTiles(tilesetFont, defaultFont) - map.DefaultRenderer.Surface.View.Size);

            foreach (var position in map.Positions())
            {
                var template = generatedMap[position] ? TerrainAtlas.Grassland : TerrainAtlas.MapEdge;
                map.SetTerrain(new Terrain(position, template.Glyph, template.Name, template.Walkable, template.Transparent));
            }

            var rng = new StandardGenerator();
            for (int i = 0; i < 10; i++)
            {
                var position = map.WalkabilityView.RandomPosition(true, rng);
                var unit = _entityFactory.CreateUnit(position, UnitAtlas.CaveTroll, sudet.Id, Color.Red);
                map.AddEntity(unit);

                position = map.WalkabilityView.RandomPosition(true, rng);
                unit = _entityFactory.CreateUnit(position, UnitAtlas.CaveTroll, blackhand.Id, Color.Blue);
                map.AddEntity(unit);
            }

            var game = new NovaGame(playerEmpire.Id, new Empire[] { sudet, blackhand });

            Game.Instance.Screen = _uiManager.CreateMapScreen(this, map, game);
            Game.Instance.DestroyDefaultStartingConsole();
            Game.Instance.Screen.IsFocused = true;
        }

        private Point GetViewportSizeInTiles(IFont tilesetFont, IFont defaultFont)
        {
            var tileSizeXFactor = (double)tilesetFont.GlyphWidth / defaultFont.GlyphWidth;
            var tileSizeYFactor = (double)tilesetFont.GlyphHeight / defaultFont.GlyphHeight;
            var viewPortTileWidth = (int)(_uiManager.ViewPortWidth / tileSizeXFactor);
            var viewPortTileHeight = (int)(_uiManager.ViewPortHeight / tileSizeYFactor);

            return new Point(viewPortTileWidth, viewPortTileHeight);
        }
    }
}
