using Newtonsoft.Json;
using Novalia.Entities;
using Novalia.Serialization.Maps;
using SadConsole;
using SadConsole.Input;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using System;
using System.Diagnostics;

namespace Novalia.Maps
{
    public enum MapEntityLayer
    {
        TERRAIN,
        TERRAINFEATURES,
        IMPROVEMENTS,
        ITEMS,
        ACTORS,
        EFFECTS,
    }

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(WorldMapJsonConverter))]
    public class WorldMap : RogueLikeMap
    {
        public WorldMap(int width, int height, IFont font)
            : base(
                  width,
                  height,
                  null,
                  Enum.GetNames(typeof(MapEntityLayer)).Length - 1,
                  Distance.Chebyshev,
                  entityLayersSupportingMultipleItems: GoRogue.SpatialMaps.LayerMasker.DEFAULT.Mask((int)MapEntityLayer.ITEMS, (int)MapEntityLayer.ACTORS, (int)MapEntityLayer.EFFECTS))
        {
            DefaultRenderer = CreateRenderer(CreateWorldMapRenderer, new Point(width, height), font, font.GetFontSize(IFont.Sizes.One));
            Font = font;
        }

        public event EventHandler SelectionChanged;

        public Unit SelectedUnit { get; private set; }

        public Point SelectedPoint { get; private set; }

        private string DebuggerDisplay => string.Format($"{nameof(WorldMap)} ({Width}, {Height})");

        public IFont Font { get; }

        private IScreenSurface CreateWorldMapRenderer(
            ICellSurface surface,
            IFont? font,
            Point? fontSize)
        {
            var renderer = new WorldMapRenderer(surface, font, fontSize.Value);
            renderer.MouseClick += Renderer_MouseClick;
            return renderer;
        }

        private void Renderer_MouseClick(object sender, MouseScreenObjectState e)
        {
            if (e.Mouse.LeftClicked)
            {
                SelectedPoint = e.CellPosition;
                SelectedUnit?.ToggleSelected();
                SelectedUnit = null;

                var clickedUnit = GetEntityAt<Unit>(e.CellPosition);
                if (clickedUnit != null)
                {
                    clickedUnit.ToggleSelected();
                    SelectedUnit = clickedUnit;
                }

                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
