using SadConsole;
using SadConsole.Input;

namespace Novalia.Maps
{
    public class MinimapScreenSurface : ScreenSurface
    {
        private readonly WorldMap _map;
        private readonly MinimapTerrainCellSurface _cellSurface;

        public MinimapScreenSurface(
            WorldMap map,
            MinimapTerrainCellSurface cellSurface,
            IFont font)
            : base(cellSurface, font)
        {
            _map = map;
            _cellSurface = cellSurface;

            UseMouse = true;
        }

        protected override void OnMouseLeftClicked(MouseScreenObjectState state)
        {
            var minimapPos = state.CellPosition;
            var mapPos = _cellSurface.MinimapToMap(minimapPos);
            _map.DefaultRenderer.Surface.View = _map.DefaultRenderer.Surface.View.WithCenter(mapPos);
        }
    }
}
