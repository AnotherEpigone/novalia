using Novalia.Serialization.Settings;
using Novalia.Ui;
using Novalia.Ui.Controls;
using SadConsole;
using SadConsole.UI;
using SadConsole.UI.Controls;
using SadConsole.UI.Themes;
using static SadConsole.UI.Colors;

namespace Novalia
{
    internal sealed class Novalia
    {
        private readonly IUiManager _uiManager;
        private readonly IGameManager _gameManager;
        private readonly IAppSettings _appSettings;

        public Novalia(IUiManager uiManager, IGameManager gameManager, IAppSettings appSettings)
        {
            _uiManager = uiManager;
            _gameManager = gameManager;
            _appSettings = appSettings;
        }

        public void Run()
        {
            Settings.AllowWindowResize = false;
            Settings.WindowTitle = "Novalia";

            Game.Create(_uiManager.ViewPortWidth, _uiManager.ViewPortHeight);
            Game.Instance.OnStart = Init;

            Game.Instance.Run();
            Game.Instance.Dispose();
        }

        private void Init()
        {
            Game.Instance.LoadFont(_uiManager.TileFontPath);
            
            // Register the types provided by the SadConsole.Extended library
            RegistrarExtended.Register();

            InitColors();
            InitControls();

            Settings.ResizeMode = Settings.WindowResizeOptions.None;

            if (_appSettings.FullScreen)
            {
                _uiManager.ToggleFullScreen();
            }
            else
            {
                _uiManager.SetViewport(_appSettings.Viewport.width, _appSettings.Viewport.height);
            }

            _uiManager.ShowMainMenu(_gameManager);
        }

        private static void InitColors()
        {
            var colors = Library.Default.Colors;

            colors.Title = new AdjustableColor(ColorHelper.TextBright, "Title", colors);
            colors.Lines = new AdjustableColor(colors.Gray, "Lines", colors);

            colors.ControlForegroundNormal = new AdjustableColor(ColorHelper.Text, "Control Foreground Normal", colors);
            colors.ControlForegroundDisabled = new AdjustableColor(colors.Gray, "Control Foreground Disabled", colors);
            colors.ControlForegroundMouseOver = new AdjustableColor(ColorHelper.TextBright, "Control Foreground MouseOver", colors);
            colors.ControlForegroundMouseDown = new AdjustableColor(ColorHelper.TextBright, "Control Foreground MouseDown", colors);
            colors.ControlForegroundSelected = new AdjustableColor(ColorHelper.TextBright, "Control Foreground Selected", colors);
            colors.ControlForegroundFocused = new AdjustableColor(ColorHelper.TextBright, "Control Foreground Focused", colors);

            colors.ControlBackgroundNormal = new AdjustableColor(ColorHelper.ControlBack, "Control Background Normal", colors);
            colors.ControlBackgroundDisabled = new AdjustableColor(ColorHelper.ControlBack, "Control Background Disabled", colors);
            colors.ControlBackgroundMouseOver = new AdjustableColor(ColorHelper.SelectedBackground, "Control Background MouseOver", colors) { Brightness = Brightness.Dark };
            colors.ControlBackgroundMouseDown = new AdjustableColor(ColorHelper.SelectedBackground, "Control Background MouseDown", colors);
            colors.ControlBackgroundSelected = new AdjustableColor(ColorHelper.SelectedBackground, "Control Background Selected", colors);
            colors.ControlBackgroundFocused = new AdjustableColor(ColorHelper.SelectedBackground, "Control Background Focused", colors) { Brightness = Brightness.Dark };

            colors.ControlHostForeground = new AdjustableColor(ColorHelper.Text, "Control Host Foreground", colors);
            colors.ControlHostBackground = new AdjustableColor(ColorHelper.ControlBack, "Control Host Background", colors);

            colors.RebuildAppearances();
        }

        private static void InitControls()
        {
            var buttonTheme = new ButtonTheme
            {
                LeftEndGlyph = 4,
                RightEndGlyph = 4,
            };

            Library.Default.SetControlTheme(typeof(McSelectionButton), buttonTheme);
            Library.Default.SetControlTheme(typeof(Button), buttonTheme);
        }
    }
}
