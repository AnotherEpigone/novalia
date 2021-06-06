using Novalia.Serialization.Settings;
using Novalia.Ui.Controls;
using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System;
using System.Diagnostics;

namespace Novalia.Ui.Consoles
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public sealed class MainMenuConsole : ScreenObject
    {
        private readonly McControlsConsole _menuConsole;
        private readonly McControlsConsole _settingsConsole;

        private McControlsConsole _activeLowerConsole;

        public MainMenuConsole(IUiManager uiManager, IGameManager gameManager, IAppSettings appSettings, int width, int height)
        {
            var titleConsole = new SadConsole.Console(width, 12);

            titleConsole.Fill(null, ColorHelper.ControlBack, null);
            titleConsole.Print(width / 6 - 6, 3, "NOVALIA", ColorHelper.Text);

            _menuConsole = CreateMenuConsole(gameManager, width, height - titleConsole.Height);
            _menuConsole.Position = new Point(0, titleConsole.Height);

            _settingsConsole = CreateSettingsConsole(uiManager, gameManager, appSettings, width, height - titleConsole.Height);
            _settingsConsole.Position = new Point(0, titleConsole.Height);
            _settingsConsole.IsVisible = false;

            Children.Add(titleConsole);
            Children.Add(_menuConsole);
            Children.Add(_settingsConsole);

            FocusConsole(_menuConsole);
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            return _menuConsole.ProcessKeyboard(info);
        }

        private void FocusConsole(McControlsConsole toFocus)
        {
            if (_activeLowerConsole != null)
            {
                _activeLowerConsole.IsVisible = false;
                _activeLowerConsole.IsFocused = false;
            }

            toFocus.IsVisible = true;
            toFocus.IsFocused = true;
            _activeLowerConsole = toFocus;
        }

        private McControlsConsole CreateSettingsConsole(IUiManager uiManager, IGameManager gameManager, IAppSettings appSettings, int width, int height)
        {
            var settingsConsole = new McControlsConsole(width, height);

            var buttonX = width / 2 - 15;
            const int topButtonY = 8;

            var fullscreenToggleButton = new McSelectionButton(30, 1)
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

            var setSize1920Button = new McSelectionButton(30, 1)
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

            var setSize1600Button = new McSelectionButton(30, 1)
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

            var setSize1280Button = new McSelectionButton(30, 1)
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

            var backButton = new McSelectionButton(20, 1)
            {
                Text = "Back",
                Position = new Point(width / 2 - 10, topButtonY + 6),
            };
            backButton.Click += (_, __) => FocusConsole(_menuConsole);

            settingsConsole.SetupSelectionButtons(
                fullscreenToggleButton,
                setSize1920Button,
                setSize1600Button,
                setSize1280Button,
                backButton);

            return settingsConsole;
        }

        private McControlsConsole CreateMenuConsole(IGameManager gameManager, int width, int height)
        {
            var menuConsole = new McControlsConsole(width, height);

            var buttonX = width / 2 - 12;
            const int topButtonY = 8;
            var continueButton = new McSelectionButton(26, 1)
            {
                IsEnabled = gameManager.CanLoad(),
                Text = "Continue",
                Position = new Point(buttonX, topButtonY),
            };
            continueButton.Click += (_, __) => gameManager.Load();

            var newGameButton = new McSelectionButton(26, 1)
            {
                Text = "New Game",
                Position = new Point(buttonX, topButtonY + 2),
            };
            newGameButton.Click += (_, __) => gameManager.StartNewGame();

            var settingsButton = new McSelectionButton(26, 1)
            {
                Text = "Settings",
                Position = new Point(buttonX, topButtonY + 4),
            };
            settingsButton.Click += (_, __) => FocusConsole(_settingsConsole);

            var exitButton = new McSelectionButton(26, 1)
            {
                Text = "Exit",
                Position = new Point(buttonX, topButtonY + 6),
            };
            exitButton.Click += (_, __) => Environment.Exit(0);

            menuConsole.SetupSelectionButtons(continueButton, newGameButton, settingsButton, exitButton);

            return menuConsole;
        }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(MainMenuConsole)} ({Position.X}, {Position.Y})");
            }
        }
    }
}
