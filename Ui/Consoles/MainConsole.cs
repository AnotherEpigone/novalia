using Novalia.Maps;
using SadConsole;
using System.Diagnostics;

namespace Novalia.Ui.Consoles
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class MainConsole : ScreenObject
    {
        public MainConsole(WorldMap map)
        {
            Children.Add(map);
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
