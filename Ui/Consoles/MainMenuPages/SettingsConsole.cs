using Novalia.Serialization.Settings;
using Novalia.Ui.Controls;
using SadRogue.Primitives;
using System;

namespace Novalia.Ui.Consoles.MainMenuPages
{
    public class SettingsConsole : NovaControlsConsole
    {
        public SettingsConsole(IUiManager uiManager, IGameManager gameManager, IAppSettings appSettings, int width, int height)
            : base(width, height)
        {
            var buttonX = width / 2 - 15;
            const int topButtonY = 8;

            var fullscreenToggleButton = new NovaSelectionButton(30, 1)
            {
                Text = "Toggle fullscreen",
                Position = new Point(buttonX, topButtonY),
            };
            fullscreenToggleButton.Click += (_, __) =>
            {
                appSettings.FullScreen = !appSettings.FullScreen;
                uiManager.ToggleFullScreen();
                uiManager.ShowMainMenu(gameManager);
            };

            var setSize1920Button = new NovaSelectionButton(30, 1)
            {
                Text = "Resize window: 1920x1080",
                Position = new Point(buttonX, topButtonY + 2),
            };
            setSize1920Button.Click += (_, __) =>
            {
                if (appSettings.FullScreen)
                {
                    appSettings.FullScreen = !appSettings.FullScreen;
                    uiManager.ToggleFullScreen();
                }

                appSettings.Viewport = (1920, 1072);
                uiManager.SetViewport(1920, 1072);
                uiManager.ShowMainMenu(gameManager);
            };

            var setSize1600Button = new NovaSelectionButton(30, 1)
            {
                Text = "Resize window: 1600x900",
                Position = new Point(buttonX, topButtonY + 3),
            };
            setSize1600Button.Click += (_, __) =>
            {
                if (appSettings.FullScreen)
                {
                    appSettings.FullScreen = !appSettings.FullScreen;
                    uiManager.ToggleFullScreen();
                }

                appSettings.Viewport = (1600, 896);
                uiManager.SetViewport(1600, 896);
                uiManager.ShowMainMenu(gameManager);
            };

            var setSize1280Button = new NovaSelectionButton(30, 1)
            {
                Text = "Resize window: 1280x720",
                Position = new Point(buttonX, topButtonY + 4),
            };
            setSize1280Button.Click += (_, __) =>
            {
                if (appSettings.FullScreen)
                {
                    appSettings.FullScreen = !appSettings.FullScreen;
                    uiManager.ToggleFullScreen();
                }

                appSettings.Viewport = (1280, 720);
                uiManager.SetViewport(1280, 720);
                uiManager.ShowMainMenu(gameManager);
            };

            var backButton = new NovaSelectionButton(20, 1)
            {
                Text = "Back",
                Position = new Point(width / 2 - 10, topButtonY + 6),
            };
            backButton.Click += (_, __) => Closed?.Invoke(this, EventArgs.Empty);

            SetupSelectionButtons(
                fullscreenToggleButton,
                setSize1920Button,
                setSize1600Button,
                setSize1280Button,
                backButton);
        }

        public event EventHandler Closed;
    }
}
