using Novalia.Maps;
using Novalia.Ui.Consoles.MainConsoleOverlays;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System.Diagnostics;

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
            IsFocused = true;

            var empireStatusConsole = new EmpireStatusConsole(40, 5)
            {
                Position = new Point(uiManager.ViewPortWidth - 40, 0),
            };

            var selectionDetailsConsole = new SelectionDetailsConsole(40, 8, Map, Game)
            {
                Position = new Point(uiManager.ViewPortWidth - 40, 5),
            };

            Map.SelectionChanged += (_, __) => selectionDetailsConsole.Update(Map, Game);

            Children.Add(Map);
            Children.Add(empireStatusConsole);
            Children.Add(selectionDetailsConsole);

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
            }

            return base.ProcessKeyboard(info);
        }
    }
}
