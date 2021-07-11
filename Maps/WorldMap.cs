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
    {        private Point _pathOverlayTarget;
        private bool _rmbDown;
        private bool _pathOverlayVisible;
        private Unit _selectedUnit;

        public WorldMap(int width, int height, IFont font, Guid playerEmpireId)
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
            PlayerEmpireId = playerEmpireId;
        }

        public event EventHandler SelectionChanged;
        public event EventHandler SelectionStatsChanged;

        public Unit SelectedUnit
        { 
            get => _selectedUnit;
            private set
            {
                if (_selectedUnit != null)
                {
                    _selectedUnit.StatsChanged -= SelectionStatsChanged;
                }

                _selectedUnit = value;
                if (_selectedUnit != null)
                {
                    _selectedUnit.StatsChanged += SelectionStatsChanged;
                }
            }
        }

        public Point SelectedPoint { get; private set; }

        private string DebuggerDisplay => string.Format($"{nameof(WorldMap)} ({Width}, {Height})");

        public IFont Font { get; }
        public Guid PlayerEmpireId { get; }

        public override void Update(TimeSpan delta)
        {
            if (_pathOverlayVisible && !_rmbDown)
            {
                MoveSelectedUnit();
                ClearPathOverlay();
            }

            if (_rmbDown)
            {
                _rmbDown = false;
            }

            base.Update(delta);
        }

        private IScreenSurface CreateWorldMapRenderer(
            ICellSurface surface,
            IFont? font,
            Point? fontSize)
        {
            var renderer = new WorldMapRenderer(surface, font, fontSize.Value);
            renderer.LeftMouseClick += Renderer_LeftMouseClick;
            renderer.RightMouseButtonDown += Renderer_RightMouseButtonDown;
            return renderer;
        }

        private void ClearPathOverlay()
        {
            _pathOverlayVisible = false;
            _pathOverlayTarget = Point.None;
            ClearGui();
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
                _rmbDown = true;
                return;
            }

            var path = AStar.ShortestPath(SelectedPoint, target);
            if (path == null)
            {
                return;
            }

            _pathOverlayTarget = target;
            _pathOverlayVisible = true;
            _rmbDown = true;

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

        private void Renderer_LeftMouseClick(object sender, MouseScreenObjectState e)
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
            if (clickedUnit != null
                && clickedUnit.EmpireId == PlayerEmpireId)
            {
                clickedUnit.ToggleSelected();
                SelectedUnit = clickedUnit;
            }

            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }

        private void MoveSelectedUnit()
        {
            if (SelectedUnit == null)
            {
                return;
            }

            var path = AStar.ShortestPath(SelectedPoint, _pathOverlayTarget);
            if (path == null)
            {
                return;
            }

            foreach (var step in path.Steps)
            {
                if (SelectedUnit.TryMove(step) != UnitMovementResult.Moved)
                {
                    break;
                }
            }

            SelectedPoint = SelectedUnit.Position;
        }
    }
}
