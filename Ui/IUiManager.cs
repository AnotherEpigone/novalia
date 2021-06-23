﻿using Novalia.Maps;
using Novalia.Ui.Consoles;
using Novalia.Ui.Windows;
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

        PopupMenuWindow CreatePopupMenu(IGameManager gameManager);

        MainConsole CreateMapScreen(
            IGameManager gameManager,
            WorldMap map,
            NovaGame game);

        void ToggleFullScreen();

        void SetViewport(int width, int height);

        int GetSidePanelWidth();

        Point GetMapConsoleSize();

        Point GetCentralWindowSize();
    }
}
