using Novalia.Ui.Controls;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;

namespace Novalia.Ui.Windows
{
    public class PopupMenuWindow : NovaControlWindow
    {
        private bool _escReleased;

        public PopupMenuWindow(IUiManager uiManager, IGameManager gameManager)
            : base(36, 13)
        {
            CloseOnEscKey = false; // it would close as soon as it opens...
            IsModalDefault = true;
            Center();
            Title = "Novalia";

            var background = new Console(Width, Height);
            background.Fill(null, ColorHelper.DarkGreyHighlight, null);

            Children.Add(background);

            const string quitText = "Exit to Desktop";
            var quitButtonWidth = quitText.Length + 4;
            var quitButton = new NovaSelectionButton(quitButtonWidth, 1)
            {
                Text = quitText,
                Position = new Point(Width / 2 - quitButtonWidth / 2, 2),
            };
            quitButton.Click += (_, __) =>
            {
                System.Environment.Exit(0);
            };

            const string mainMenuText = "Exit to Main Menu";
            var mainMenuButtonWidth = mainMenuText.Length + 4;
            var mainMenuButton = new NovaSelectionButton(mainMenuButtonWidth, 1)
            {
                Text = mainMenuText,
                Position = new Point(Width / 2 - mainMenuButtonWidth / 2, 4),
            };
            mainMenuButton.Click += (_, __) =>
            {
                Hide();
                uiManager.ShowMainMenu(gameManager);
            };

            const string saveText = "Save";
            var saveButtonWidth = saveText.Length + 4;
            var saveButton = new NovaSelectionButton(saveButtonWidth, 1)
            {
                Text = saveText,
                Position = new Point(Width / 2 - saveButtonWidth / 2, 6),
            };
            saveButton.Click += (_, __) =>
            {
                Hide();
                gameManager.Save();
            };

            const string loadText = "Load";
            var loadButtonWidth = loadText.Length + 4;
            var loadButton = new NovaSelectionButton(loadButtonWidth, 1)
            {
                Text = loadText,
                Position = new Point(Width / 2 - loadButtonWidth / 2, 8),
            };
            loadButton.Click += (_, __) =>
            {
                Hide();
                gameManager.Load();
            };

            const string closeText = "Return to Game";
            var closeButtonWidth = closeText.Length + 4;
            var closeButton = new NovaSelectionButton(closeButtonWidth, 1)
            {
                Text = closeText,
                Position = new Point(Width / 2 - closeButtonWidth / 2, 10),
            };
            closeButton.Click += (_, __) => Hide();

            SetupSelectionButtons(quitButton, mainMenuButton, saveButton, loadButton, closeButton);
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            if (!info.IsKeyPressed(Keys.Escape))
            {
                _escReleased = true;
                return base.ProcessKeyboard(info);
            }

            if (_escReleased)
            {
                Hide();
                return true;
            }

            return base.ProcessKeyboard(info);
        }
    }
}
