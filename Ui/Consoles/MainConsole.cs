using Novalia.Maps;
using Novalia.Ui.Consoles.MainConsoleOverlays;
using SadConsole;
using System.Diagnostics;

namespace Novalia.Ui.Consoles
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class MainConsole : ScreenObject
    {
        public MainConsole(IUiManager uiManager, WorldMap map)
        {
            var topLeftInfoConsole = new TopLeftInfoConsole(40, 3);

            Children.Add(map);
            Children.Add(topLeftInfoConsole);
        }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(MainConsole)} ({Position.X}, {Position.Y})");
            }
        }
    }
}
