using SadConsole;
using SadRogue.Primitives;
using System.Diagnostics;
using System.Linq;

namespace Novalia.Ui.Consoles
{
    /// <summary>
    /// Displays a single-line message that flashes.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class AlertMessageConsole : Console
    {
        public AlertMessageConsole(int width)
            : base(width, 1)
        {
            Surface.DefaultBackground = Color.Black.SetAlpha(100);

            IsVisible = false;
        }

        public void Hide()
        {
            IsVisible = false;
        }

        public void Show(string message)
        {
            IsVisible = true;
            Surface.Clear();
            Cursor.Position = new Point(0, 0);
            var coloredMessage = new ColoredString(message, Color.Gainsboro, DefaultBackground)
            {
                IgnoreEffect = false,
            };

            // TODO make the flash work.
            var effect = new SadConsole.Effects.Fade
            {
                AutoReverse = true,
                DestinationForeground = new ColorGradient(Color.Gainsboro, Color.Transparent),
                FadeForeground = true,
                UseCellForeground = false,
                Repeat = true,
                FadeDuration = 0.7f,
                RemoveOnFinished = true
            };
            Surface.Effects.SetEffect(Enumerable.Range(0, Surface.Area.Area - 1), effect);
            Surface.IsDirty = true;

            Cursor.Print(coloredMessage);
        }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(AlertMessageConsole)} ({Position.X}, {Position.Y})");
            }
        }
    }
}
