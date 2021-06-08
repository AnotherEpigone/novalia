using Novalia.Maps;
using Novalia.Ui.Consoles;
using SadConsole;
using SadRogue.Primitives;

namespace Novalia.Ui
{
    public interface IUiManager
    {
        int ViewPortHeight { get; }
        int ViewPortWidth { get; }
        string TileFontPath { get; }
        string TileFontName { get; }

        void ShowMainMenu(IGameManager gameManager);

        MainConsole CreateMapScreen(
            WorldMap map);

        void ToggleFullScreen();

        void SetViewport(int width, int height);

        int GetSidePanelWidth();

        Point GetMapConsoleSize();

        Point GetCentralWindowSize();
    }
}
