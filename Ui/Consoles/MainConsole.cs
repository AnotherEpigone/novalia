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
        private readonly WorldMap _map;
        private readonly NovaGame _game;

        public MainConsole(
            IGameManager gameManager,
            IUiManager uiManager,
            WorldMap map,
            NovaGame game,
            bool debug)
        {
            _gameManager = gameManager;
            _uiManager = uiManager;
            _map = map;
            _game = game;
            UseMouse = false;
            IsFocused = true;

            var empireStatusConsole = new EmpireStatusConsole(40, 5)
            {
                Position = new Point(uiManager.ViewPortWidth - 40, 0),
            };

            var selectionDetailsConsole = new SelectionDetailsConsole(40, 8, _map, _game)
            {
                Position = new Point(uiManager.ViewPortWidth - 40, 5),
            };

            _map.SelectionChanged += (_, __) => selectionDetailsConsole.Update(_map, _game);

            Children.Add(_map);
            Children.Add(empireStatusConsole);
            Children.Add(selectionDetailsConsole);

            if (debug)
            {
                ////SadComponents.Add(new MouseTint());
            }
        }

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
