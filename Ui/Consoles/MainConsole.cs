using Novalia.Maps;
using Novalia.Ui.Consoles.MainConsoleOverlays;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System.Diagnostics;
using System.Linq;

namespace Novalia.Ui.Consoles
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class MainConsole : ScreenObject
    {
        private readonly IGameManager _gameManager;
        private readonly IUiManager _uiManager;

        public MainConsole(
            IGameManager gameManager,
            IUiManager uiManager,
            WorldMap map,
            NovaGame game,
            bool debug)
        {
            _gameManager = gameManager;
            _uiManager = uiManager;
            Map = map;
            Game = game;
            UseMouse = false;
            UseKeyboard = true;

            var empireStatusConsole = new EmpireStatusConsole(40, 5)
            {
                Position = new Point(uiManager.ViewPortWidth - 40, 0),
            };

            var selectionDetailsConsole = new SelectionDetailsConsole(40, 9, Map, Game)
            {
                Position = new Point(uiManager.ViewPortWidth - 40, 5),
            };

            var minimap = new ScreenSurface(
                new MinimapTerrainCellSurface(Map, 320, 320),
                SadConsole.Game.Instance.Fonts[uiManager.MiniMapFontName]);
            var minimapGlyphPosition = new Point(uiManager.ViewPortWidth - 40, 9 + 6);
            minimap.Position = new Point(
                minimapGlyphPosition.X * SadConsole.Game.Instance.DefaultFont.GlyphWidth,
                minimapGlyphPosition.Y * SadConsole.Game.Instance.DefaultFont.GlyphHeight);

            Map.SelectionChanged += (_, __) => selectionDetailsConsole.Update(Map, Game);
            Map.SelectionStatsChanged += (_, __) => selectionDetailsConsole.Update(Map, Game);

            Children.Add(Map);
            Children.Add(empireStatusConsole);
            Children.Add(selectionDetailsConsole);
            Children.Add(minimap);

            if (debug)
            {
                ////SadComponents.Add(new MouseTint());
            }
        }

        public WorldMap Map { get; }

        public NovaGame Game { get; }

        private string DebuggerDisplay => string.Format($"{nameof(MainConsole)} ({Position.X}, {Position.Y})");

        public override bool ProcessKeyboard(Keyboard info)
        {
            if (info.IsKeyPressed(Keys.Escape))
            {
                _uiManager.CreatePopupMenu(_gameManager).Show(true);
                return true;
            }

            if (Map.HandleKeyboard(info))
            {
                return true;
            }

            return base.ProcessKeyboard(info);
        }
    }
}
