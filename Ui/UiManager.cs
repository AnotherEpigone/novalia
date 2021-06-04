using Novalia.Ui.Consoles;
using SadConsole;
using SadRogue.Primitives;

namespace Novalia.Ui
{
    public sealed class UiManager : IUiManager
    {
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
            throw new System.NotImplementedException();
        }

        public void ShowMainMenu(IGameManager gameManager)
        {
            var menu = new MainMenuConsole(this, gameManager, ViewPortWidth, ViewPortHeight);
            Game.Instance.Screen = menu;
            menu.IsFocused = true;
        }

        public void ToggleFullScreen()
        {
            throw new System.NotImplementedException();
        }
    }
}
