using Novalia.Entities;
using Novalia.GameMechanics;
using Novalia.GameMechanics.Setup;
using Novalia.Logging;
using Novalia.Maps;
using Novalia.Maps.Generation;
using Novalia.Serialization;
using Novalia.Ui;
using Novalia.Ui.Consoles;
using SadConsole;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using System.Collections.Generic;
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
        private readonly ITurnManagerFactory _turnManagerFactory;

        public GameManager(
            IUiManager uiManager,
            IEntityFactory entityFactory,
            ILogger logger,
            ISaveManager saveManager,
            ITurnManagerFactory turnManagerFactory)
        {
            _uiManager = uiManager;
            _entityFactory = entityFactory;
            _logger = logger;
            _saveManager = saveManager;
            _turnManagerFactory = turnManagerFactory;
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

            var tilesetFont = Game.Instance.Fonts[_uiManager.TileFontName];
            var defaultFont = Game.Instance.DefaultFont;
            var game = new NovaGame(
                gameState.Empires,
                _turnManagerFactory.Create(gameState.Turn, gameState.Empires));
            var map = gameState.Map;
            map.DefaultRenderer.Surface.View = map.DefaultRenderer.Surface.View.ChangeSize(
                GetViewportSizeInTiles(tilesetFont, defaultFont) - map.DefaultRenderer.Surface.View.Size);

            var mapManager = new WorldMapManager(game, map);
            mapManager.SelectNextUnit();

            Game.Instance.Screen = _uiManager.CreateMapScreen(this, gameState.Map, mapManager, game);
            Game.Instance.DestroyDefaultStartingConsole();
            Game.Instance.Screen.IsFocused = true;
        }

        public void LoadLatest()
        {
            Load();
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
                Map = mainConsole.Map,
                Turn = mainConsole.Game.TurnManager.Turn,
            };

            _saveManager.Write(save);
        }

        public void StartNewGame(GameSetup setup)
        {
            _logger.Debug("Starting new game.");
            var tilesetFont = Game.Instance.Fonts[_uiManager.TileFontName];
            var defaultFont = Game.Instance.DefaultFont;

            var rng = new StandardGenerator();

            var blackhand = new Empire(EmpireAtlas.BlackhandDominion);

            var allEmpires = new List<Empire>(setup.PlayerEmpires)
            {
                blackhand,
            };

            var turnManager = _turnManagerFactory.Create(0, allEmpires);

            var mapFactory = new WorldMapFactory();
            var map = mapFactory.Create(
                setup.MapGenerationSettings,
                tilesetFont,
                GetViewportSizeInTiles(tilesetFont, defaultFont),
                rng);

            for (int i = 0; i < 2; i++)
            {
                Point position;
                NovaEntity unit;
                foreach (var playerEmpire in setup.PlayerEmpires)
                {
                    position = map.WalkabilityView.RandomPosition(true, rng);
                    unit = _entityFactory.CreateUnit(position, UnitAtlas.CaveTroll, playerEmpire.Id, playerEmpire.Color);
                    map.AddEntity(unit);
                }

                position = map.WalkabilityView.RandomPosition(true, rng);
                unit = _entityFactory.CreateUnit(position, UnitAtlas.CaveTroll, blackhand.Id, blackhand.Color);
                map.AddEntity(unit);
            }

            var game = new NovaGame(
                allEmpires,
                turnManager);

            var mapManager = new WorldMapManager(game, map);
            mapManager.SelectNextUnit();

            Game.Instance.Screen = _uiManager.CreateMapScreen(this, map, mapManager, game);
            Game.Instance.DestroyDefaultStartingConsole();
            Game.Instance.Screen.IsFocused = true;
        }

        private Point GetViewportSizeInTiles(IFont tilesetFont, IFont defaultFont)
        {
            var tileSizeXFactor = (double)tilesetFont.GlyphWidth / defaultFont.GlyphWidth;
            var tileSizeYFactor = (double)tilesetFont.GlyphHeight / defaultFont.GlyphHeight;
            var viewPortTileWidth = (int)(_uiManager.ViewPortWidth / tileSizeXFactor);
            var viewPortTileHeight = (int)(_uiManager.ViewPortHeight / tileSizeYFactor);

            var rightPaneWidth = (int)(MainConsole.RightPaneWidth / tileSizeXFactor);
            var mapWidth = viewPortTileWidth - rightPaneWidth;

            return new Point(mapWidth, viewPortTileHeight);
        }
    }
}
