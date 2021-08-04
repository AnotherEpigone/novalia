using SadConsole;
using SadRogue.Primitives;
using System;
using System.Diagnostics;
using System.Linq;

namespace Novalia.Ui.Consoles
{
    /// <summary>
    /// Displays a single-line message that flashes.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class AlertMessageConsole : SadConsole.Console
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

        public override void Update(TimeSpan delta)
        {
            base.Update(delta);
        }

        public void Show(string message)
        {
            IsVisible = true;
            Surface.Clear();
            Cursor.Position = new Point(0, 0);
            var coloredMessage = new ColoredString(message, ColorHelper.Text, DefaultBackground)
            {
                IgnoreEffect = false,
            };

            var effect = new SadConsole.Effects.Fade
            {
                StartDelay = 0.2d,
                AutoReverse = true,
                DestinationForeground = new ColorGradient(ColorHelper.Text, ColorHelper.TextDisabled),
                FadeForeground = true,
                UseCellForeground = false,
                Repeat = true,
                FadeDuration = 1d
            };

            coloredMessage.SetEffect(effect);
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
