using GoRogue.MapGeneration;
using Novalia.Entities;
using Novalia.GameMechanics;
using Novalia.Logging;
using Novalia.Maps;
using Novalia.Maps.Generation;
using Novalia.Maps.Generation.Steps;
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
                gameState.PlayerEmpireId,
                gameState.Empires,
                _turnManagerFactory.Create(gameState.Turn));
            var map = gameState.Map;
            map.DefaultRenderer.Surface.View = map.DefaultRenderer.Surface.View.ChangeSize(
                GetViewportSizeInTiles(tilesetFont, defaultFont) - map.DefaultRenderer.Surface.View.Size);

            Game.Instance.Screen = _uiManager.CreateMapScreen(this, gameState.Map, game);
            Game.Instance.DestroyDefaultStartingConsole();
            Game.Instance.Screen.IsFocused = true;

            map.SelectNextUnit();
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
                PlayerEmpireId = mainConsole.Game.PlayerEmpireId,
                Map = mainConsole.Map,
                Turn = mainConsole.Game.TurnManager.Turn,
            };

            _saveManager.Write(save);
        }

        public void StartNewGame(MapGenerationSettings settings)
        {
            _logger.Debug("Starting new game.");
            var tilesetFont = Game.Instance.Fonts[_uiManager.TileFontName];
            var defaultFont = Game.Instance.DefaultFont;

            var rng = new StandardGenerator();

            var sudet = new Empire(EmpireAtlas.Sudet);
            var blackhand = new Empire(EmpireAtlas.BlackhandDominion);
            var playerEmpire = sudet;

            var mapFactory = new WorldMapFactory();
            var map = mapFactory.Create(settings, tilesetFont, GetViewportSizeInTiles(tilesetFont, defaultFont), playerEmpire.Id, rng);

            for (int i = 0; i < 2; i++)
            {
                var position = map.WalkabilityView.RandomPosition(true, rng);
                var unit = _entityFactory.CreateUnit(position, UnitAtlas.CaveTroll, sudet.Id, Color.Red);
                map.AddEntity(unit);

                position = map.WalkabilityView.RandomPosition(true, rng);
                unit = _entityFactory.CreateUnit(position, UnitAtlas.CaveTroll, blackhand.Id, Color.Blue);
                map.AddEntity(unit);
            }

            var game = new NovaGame(
                playerEmpire.Id,
                new Empire[] { sudet, blackhand },
                _turnManagerFactory.Create(0));

            Game.Instance.Screen = _uiManager.CreateMapScreen(this, map, game);
            Game.Instance.DestroyDefaultStartingConsole();
            Game.Instance.Screen.IsFocused = true;

            map.SelectNextUnit();
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
