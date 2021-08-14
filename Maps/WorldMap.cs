using GoRogue.Pathing;
using Newtonsoft.Json;
using Novalia.Entities;
using Novalia.Fonts;
using Novalia.Serialization.Maps;
using SadConsole;
using SadConsole.Input;
using SadRogue.Integration.Maps;
using SadRogue.Primitives;
using System;
using System.Diagnostics;
using System.Linq;

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
        GUI,
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
                  Enum.GetNames(typeof(MapEntityLayer)).Length,
                  Distance.Chebyshev,
                  entityLayersSupportingMultipleItems: GoRogue.SpatialMaps.LayerMasker.DEFAULT.Mask((int)MapEntityLayer.ITEMS, (int)MapEntityLayer.ACTORS, (int)MapEntityLayer.EFFECTS, (int)MapEntityLayer.GUI))
        {
            Font = font;

            DefaultRenderer = CreateRenderer(CreateWorldMapRenderer, new Point(width, height), font, font.GetFontSize(IFont.Sizes.One));
        }

        public event EventHandler<MouseScreenObjectState> RightMouseButtonDown;
        public event EventHandler<MouseScreenObjectState> LeftMouseClick;

        private string DebuggerDisplay => string.Format($"{nameof(WorldMap)} ({Width}, {Height})");

        public IFont Font { get; }

        public IScreenSurface CreateMinimapRenderer(int pixelWidth, int pixelHeight, IFont pixelFont)
        {
            var cellSurface = new MapTerrainCellSurface(this, pixelWidth, pixelHeight);
            return new ScreenSurface(cellSurface, pixelFont);
        }

        private IScreenSurface CreateWorldMapRenderer(
            ICellSurface surface,
            IFont font,
            Point? fontSize)
        {
            var renderer = new WorldMapRenderer(surface, font, fontSize.Value);
            renderer.LeftMouseClick += (s, e) => LeftMouseClick?.Invoke(s, e);
            renderer.RightMouseButtonDown += (s, e) => RightMouseButtonDown?.Invoke(s, e);
            renderer.UseMouse = true;
            return renderer;
        }
    }
}
