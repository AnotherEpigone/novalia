using Novalia.Entities;
using Novalia.Maps;
using Novalia.Ui.Consoles.MainConsoleOverlays;
using SadConsole;
using SadConsole.Input;
using SadRogue.Integration.Components.SadConsole;
using System.Diagnostics;

namespace Novalia.Ui.Consoles
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class MainConsole : ScreenObject
    {
        private readonly WorldMap _map;

        public MainConsole(IUiManager uiManager, WorldMap map, bool debug)
        {
            _map = map;

            var topLeftInfoConsole = new TopLeftInfoConsole(40, 3);

            UseMouse = false;

            Children.Add(_map);
            Children.Add(topLeftInfoConsole);

            if (debug)
            {
                ////SadComponents.Add(new MouseTint());
            }
        }

        private string DebuggerDisplay => string.Format($"{nameof(MainConsole)} ({Position.X}, {Position.Y})");

        public override bool ProcessKeyboard(Keyboard info)
        {
            return base.ProcessKeyboard(info);
        }
    }
}
