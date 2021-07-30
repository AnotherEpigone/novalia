using Novalia.Maps.Generation;
using Novalia.Serialization.Settings;
using Novalia.Ui.Consoles.MainMenuPages;
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
        private readonly NovaControlsConsole _menuConsole;
        private readonly SettingsConsole _settingsConsole;
        private readonly MapOptionsConsole _mapOptionsConsole;

        private NovaControlsConsole _activeLowerConsole;
        private MapGenerationSettings _mapSettings;

        public MainMenuConsole(IUiManager uiManager, IGameManager gameManager, IAppSettings appSettings, int width, int height)
        {
            var titleConsole = new SadConsole.Console(width, 12);

            titleConsole.Fill(null, ColorHelper.ControlBack, null);
            titleConsole.Print(width / 6 - 6, 3, "NOVALIA", ColorHelper.Text);

            _menuConsole = CreateMenuConsole(gameManager, width, height - titleConsole.Height);
            _menuConsole.Position = new Point(0, titleConsole.Height);

            _settingsConsole = new SettingsConsole(uiManager, gameManager, appSettings, width, height - titleConsole.Height)
            {
                Position = new Point(0, titleConsole.Height),
                IsVisible = false,
            };
            _settingsConsole.Closed += (_, __) => FocusConsole(_menuConsole);

            _mapOptionsConsole = new MapOptionsConsole(gameManager, width, height - titleConsole.Height)
            {
                Position = new Point(0, titleConsole.Height),
                IsVisible = false,
            };
            _mapOptionsConsole.Closed += (_, __) => FocusConsole(_menuConsole);

            Children.Add(titleConsole);
            Children.Add(_menuConsole);
            Children.Add(_settingsConsole);
            Children.Add(_mapOptionsConsole);

            FocusConsole(_menuConsole);
        }

        public override bool ProcessKeyboard(Keyboard info)
        {
            return _menuConsole.ProcessKeyboard(info);
        }

        private void FocusConsole(NovaControlsConsole toFocus)
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

        private NovaControlsConsole CreateMenuConsole(IGameManager gameManager, int width, int height)
        {
            var menuConsole = new NovaControlsConsole(width, height);

            var buttonX = width / 2 - 12;
            const int topButtonY = 8;
            var continueButton = new NovaSelectionButton(26, 1)
            {
                IsEnabled = gameManager.CanLoad(),
                Text = "Continue",
                Position = new Point(buttonX, topButtonY),
            };
            continueButton.Click += (_, __) => gameManager.LoadLatest();

            var newGameButton = new NovaSelectionButton(26, 1)
            {
                Text = "New Game",
                Position = new Point(buttonX, topButtonY + 2),
            };
            newGameButton.Click += (_, __) =>
            {

                FocusConsole(_mapOptionsConsole);
            };

            var loadButton = new NovaSelectionButton(26, 1)
            {
                Text = "New Game",
                Position = new Point(buttonX, topButtonY + 2),
            };
            //loadButton.Click += (_, __) => FocusConsole(_loadConsole);

            var settingsButton = new NovaSelectionButton(26, 1)
            {
                Text = "Settings",
                Position = new Point(buttonX, topButtonY + 4),
            };
            settingsButton.Click += (_, __) => FocusConsole(_settingsConsole);

            var exitButton = new NovaSelectionButton(26, 1)
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
