using Novalia.Serialization.Settings;
using Novalia.Ui.Consoles;
using SadConsole;
using SadRogue.Primitives;

namespace Novalia.Ui
{
    public sealed class UiManager : IUiManager
    {
        private readonly IAppSettings _appSettings;

        public UiManager(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public int ViewPortWidth { get; private set; } = 160; // 160 x 8 = 1280
        public int ViewPortHeight { get; private set; } = 45; // 45 x 16 = 720

        public string TileFontPath { get; } = "Fonts\\tiles.font";
        public string TileFontName { get; } = "Tiles";

        public MainConsole CreateMapScreen(IGameManager gameManager, Font tilesetFont)
        {
            throw new System.NotImplementedException();
        }

        public Point GetCentralWindowSize()
        {
            throw new System.NotImplementedException();
        }

        public Point GetMapConsoleSize()
        {
            throw new System.NotImplementedException();
        }

        public int GetSidePanelWidth()
        {
            throw new System.NotImplementedException();
        }

        public void SetViewport(int width, int height)
        {
            Game.Instance.ResizeWindow(width, height);

            RefreshViewport();
        }

        public void ShowMainMenu(IGameManager gameManager)
        {
            var menu = new MainMenuConsole(this, gameManager, _appSettings, ViewPortWidth, ViewPortHeight);
            Game.Instance.Screen = menu;
        }

        public void ToggleFullScreen()
        {
            Game.Instance.ToggleFullScreen();
            Game.Instance.MonoGameInstance.ResetRendering();

            RefreshViewport();
        }

        private void RefreshViewport()
        {
            ViewPortWidth = SadConsole.Host.Global.GraphicsDevice.PresentationParameters.BackBufferWidth / Game.Instance.DefaultFont.GetFontSize(Font.Sizes.One).X;
            ViewPortHeight = SadConsole.Host.Global.GraphicsDevice.PresentationParameters.BackBufferHeight / Game.Instance.DefaultFont.GetFontSize(Font.Sizes.One).Y;
        }
    }
}
