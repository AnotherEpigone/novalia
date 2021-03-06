using Novalia.Entities;
using Novalia.GameMechanics;
using Novalia.GameMechanics.Combat;
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
        private readonly ICombatManagerFactory _combatManagerFactory;

        public GameManager(
            IUiManager uiManager,
            IEntityFactory entityFactory,
            ILogger logger,
            ISaveManager saveManager,
            ITurnManagerFactory turnManagerFactory,
            ICombatManagerFactory combatManagerFactory)
        {
            _uiManager = uiManager;
            _entityFactory = entityFactory;
            _logger = logger;
            _saveManager = saveManager;
            _turnManagerFactory = turnManagerFactory;
            _combatManagerFactory = combatManagerFactory;
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

            var rng = new StandardGenerator();
            var tilesetFont = Game.Instance.Fonts[_uiManager.TileFontName];
            var defaultFont = Game.Instance.DefaultFont;
            var game = new NovaGame(
                gameState.Empires,
                _turnManagerFactory.Create(gameState.Turn, gameState.Empires),
                _combatManagerFactory.Create(rng));
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
                Turn = mainConsole.Game.TurnManager.Round,
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

            var mapFactory = new WorldMapFactory();
            var map = mapFactory.Create(
                setup.MapGenerationSettings,
                tilesetFont,
                GetViewportSizeInTiles(tilesetFont, defaultFont),
                rng,
                _entityFactory);

            for (int i = 0; i < 5; i++)
            {
                foreach (var playerEmpire in setup.PlayerEmpires)
                {
                    var position = map.WalkabilityView.RandomPosition(true, rng);
                    var unit = _entityFactory.CreateUnit(position, UnitAtlas.Spearman, playerEmpire.Id, playerEmpire.Color);
                    map.AddEntity(unit);
                }
            }

            foreach (var playerEmpire in setup.PlayerEmpires)
            {
                var position = map.WalkabilityView.RandomPosition(true, rng);
                var city = _entityFactory.CreateCity(position, "Anathema", CityAtlas.HumanTown, playerEmpire.Id, playerEmpire.Color);
                map.AddEntity(city);
            }


            for (int i = 0; i < 2; i++)
            {
                var position = map.WalkabilityView.RandomPosition(true, rng);
                var unit = _entityFactory.CreateUnit(position, UnitAtlas.CaveTroll, blackhand.Id, blackhand.Color);
                map.AddEntity(unit);
            }

            var game = new NovaGame(
                allEmpires,
                _turnManagerFactory.Create(0, allEmpires),
                _combatManagerFactory.Create(rng));

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
