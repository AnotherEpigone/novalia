using Novalia.Ui.Controls;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;

namespace Novalia.Ui.Windows
{
    public class NovaMessageBox : NovaControlWindow
    {
        private bool _debounced;

        public NovaMessageBox(string title, string message, string closeButtonText)
            : base(message.Length + 4, 7)
        {
            CloseOnEscKey = false; // needs to be debounced
            IsModalDefault = true;
            Center();
            Title = title;

            var background = new Console(Width, Height);
            background.Fill(null, ColorHelper.DarkGreyHighlight, null);
            background.Surface.Print(2, 2, message);

            Children.Add(background);

            var closeButtonWidth = closeButtonText.Length + 4;
            var closeButton = new NovaSelectionButton(closeButtonWidth, 1)
            {
                Text = closeButtonText,
                Position = new Point(Width / 2 - closeButtonWidth / 2, 4),
            };
            closeButton.Click += (_, __) =>
            {
                if (_debounced)
                {
                    Hide();
                }
            };

            SetupSelectionButtons(closeButton);
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            if (!info.IsKeyDown(Keys.Escape) && !info.IsKeyDown(Keys.Enter) && !_debounced)
            {
                _debounced = true;
                return true;
            }

            if (!_debounced)
            {
                return base.ProcessKeyboard(info);
            }

            if (info.IsKeyPressed(Keys.Escape))
            {
                Hide();
                return true;
            }

            return base.ProcessKeyboard(info);
        }

        public static void Show(string title, string message, string closeButtonText)
        {
            var window = new NovaMessageBox(title, message, closeButtonText);
            window.Show(true);
        }
    }
}
