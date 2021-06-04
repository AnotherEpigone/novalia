using Novalia.Ui;
using Novalia.Ui.Controls;
using SadConsole;
using SadConsole.UI.Controls;
using SadConsole.UI.Themes;

namespace Novalia
{
    internal sealed class Novalia
    {
        private readonly IUiManager _uiManager;
        private readonly IGameManager _gameManager;

        public Novalia(IUiManager uiManager, IGameManager gameManager)
        {
            _uiManager = uiManager;
            _gameManager = gameManager;
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
            //Game.Instance.LoadFont(_uiManager.TileFontPath);
            
            // Register the types provided by the SadConsole.Extended library
            SadConsole.UI.RegistrarExtended.Register();

            InitColors();
            InitControls();

            Settings.ResizeMode = Settings.WindowResizeOptions.None;
            // Settings.FullScreenPreventScaleChangeForNone = true;

            //if (_appSettings.FullScreen)
            //{
            //    _uiManager.ToggleFullScreen();
            //}
            //else
            //{
            //    _uiManager.SetViewport(_appSettings.Viewport.width, _appSettings.Viewport.height);
            //}

            _uiManager.ShowMainMenu(_gameManager);
        }

        private static void InitColors()
        {
            var colors = Library.Default.Colors;

            ////colors.Title = ColorHelper.TextBright;
            ////colors.Text = ColorHelper.Text;
            ////colors.TextSelected = ColorHelper.TextBright;
            ////colors.TextSelectedDark = ColorHelper.TextBright;
            ////colors.TextLight = ColorHelper.SelectedBackground;
            ////colors.TextDark = ColorHelper.TextBright;
            ////colors.TextFocused = ColorHelper.TextBright;

            ////colors.Lines = colors.Gray;

            ////colors.ControlBack = ColorHelper.ControlBack;
            ////colors.ControlBackLight = ColorHelper.SelectedBackground;
            ////colors.ControlBackSelected = ColorHelper.SelectedBackground;
            ////colors.ControlBackDark = ColorHelper.ControlBack;
            ////colors.ControlHostBack = ColorHelper.ControlBack;
            ////colors.ControlHostFore = ColorHelper.Text;

            colors.RebuildAppearances();
        }

        private static void InitControls()
        {
            var buttonTheme = new ButtonTheme
            {
                ShowEnds = false,
            };

            Library.Default.SetControlTheme(typeof(McSelectionButton), buttonTheme);
            Library.Default.SetControlTheme(typeof(Button), buttonTheme);
        }
    }
}
