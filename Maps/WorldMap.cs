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
        private Point _pathOverlayTarget;
        private bool _keepPathOverlay;
        private bool _pathOverlayVisible;

        public WorldMap(int width, int height, IFont font)
            : base(
                  width,
                  height,
                  null,
                  Enum.GetNames(typeof(MapEntityLayer)).Length,
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

        public override void Update(TimeSpan delta)
        {
            if (_pathOverlayVisible && !_keepPathOverlay)
            {
                _pathOverlayVisible = false;
                _pathOverlayTarget = Point.None;
                ClearGui();
            }

            if (_keepPathOverlay)
            {
                _keepPathOverlay = false;
            }

            base.Update(delta);
        }

        private IScreenSurface CreateWorldMapRenderer(
            ICellSurface surface,
            IFont? font,
            Point? fontSize)
        {
            var renderer = new WorldMapRenderer(surface, font, fontSize.Value);
            renderer.MouseClick += Renderer_MouseClick;
            renderer.RightMouseButtonDown += Renderer_RightMouseButtonDown;
            return renderer;
        }

        private void ClearGui()
        {
            var overlayEntities = Entities.GetLayer((int)MapEntityLayer.GUI).Items;
            foreach (var entity in overlayEntities)
            {
                RemoveEntity(entity);
            }
        }

        private void Renderer_RightMouseButtonDown(object sender, MouseScreenObjectState e)
        {
            if (SelectedUnit == null)
            {
                return;
            }

            var target = e.CellPosition;
            if (_pathOverlayTarget == target)
            {
                // don't recalculate for the same target
                _keepPathOverlay = true;
                return;
            }

            var path = AStar.ShortestPath(SelectedPoint, target);
            if (path == null)
            {
                return;
            }

            _pathOverlayTarget = target;
            _pathOverlayVisible = true;
            _keepPathOverlay = true;

            ClearGui();
            foreach (var step in path.Steps)
            {
                AddEntity(new NovaEntity(
                    step,
                    new Color(Color.White, 100),
                    GlyphAtlas.Solid,
                    "step highlight",
                    true,
                    true,
                    (int)MapEntityLayer.GUI,
                    Guid.NewGuid()));
            }
        }

        private void Renderer_MouseClick(object sender, MouseScreenObjectState e)
        {
            if (e.Mouse.LeftClicked)
            {
                SelectedUnit?.ToggleSelected();
                SelectedUnit = null;
                if (SelectedPoint == e.CellPosition)
                {
                    SelectedPoint = Point.None;
                    SelectionChanged?.Invoke(this, EventArgs.Empty);
                    return;
                }

                SelectedPoint = e.CellPosition;                

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
