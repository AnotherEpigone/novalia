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
        private Point _pathOverlayTarget;
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
                  entityLayersSupportingMultipleItems: GoRogue.SpatialMaps.LayerMasker.DEFAULT.Mask((int)MapEntityLayer.ITEMS, (int)MapEntityLayer.ACTORS, (int)MapEntityLayer.EFFECTS, (int)MapEntityLayer.GUI))
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

        public bool HandleKeyboard(Keyboard keyboard)
        {
            if (keyboard.IsKeyPressed(Keys.Enter))
            {
                var moveableUnit = Entities.Items
                    .OfType<Unit>()
                    .Where(e => e.EmpireId == PlayerEmpireId
                        && e.RemainingMovement > 0.01
                        && e != SelectedUnit)
                    .OrderBy(u => u.LastSelected)
                    .FirstOrDefault();
                if (moveableUnit != null)
                {
                    SelectedUnit?.ToggleSelected();
                    SelectedUnit = moveableUnit;
                    SelectedPoint = SelectedUnit.Position;
                    SelectedUnit.ToggleSelected();
                    DefaultRenderer.Surface.View = DefaultRenderer.Surface.View.WithCenter(SelectedPoint);
                }

                return true;
            }

            if (keyboard.IsKeyPressed(Keys.C))
            {
                DefaultRenderer.Surface.View = DefaultRenderer.Surface.View.WithCenter(SelectedPoint);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Down))
            {
                DefaultRenderer.Surface.View = DefaultRenderer.Surface.View.ChangePosition(Direction.Down);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Up))
            {
                DefaultRenderer.Surface.View = DefaultRenderer.Surface.View.ChangePosition(Direction.Up);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Right))
            {
                DefaultRenderer.Surface.View = DefaultRenderer.Surface.View.ChangePosition(Direction.Right);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Left))
            {
                DefaultRenderer.Surface.View = DefaultRenderer.Surface.View.ChangePosition(Direction.Left);
                return true;
            }

            return false;
        }

        public override void Update(TimeSpan delta)
        {
            if (_pathOverlayVisible && !_rmbDown)
            {
                MoveSelectedUnit(_pathOverlayTarget);
                ClearPathOverlay();
            }

            _rmbDown = false;

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
            renderer.UseMouse = true;
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

        private void AddTurnCountEntities(int turns, Point point)
        {
            var alpha = turns > 1
                        ? 150
                        : 210;
            if (turns < 10)
            {
                AddEntity(new NovaEntity(
                    point,
                    new Color(Color.White, 210),
                    GlyphAtlas.MovementPreview1 + turns - 1,
                    "step highlight - turn count",
                    true,
                    true,
                    (int)MapEntityLayer.GUI,
                    Guid.NewGuid()));
            }
            else if (turns > 99)
            {
                AddEntity(new NovaEntity(
                    point,
                    new Color(Color.White, 150),
                    GlyphAtlas.MovementPreview99plus,
                    "step highlight - turn count",
                    true,
                    true,
                    (int)MapEntityLayer.GUI,
                    Guid.NewGuid()));
            }
            else
            {
                // tens digit.
                AddEntity(new NovaEntity(
                    point,
                    new Color(Color.White, 150),
                    GlyphAtlas.MovementPreview10 + (turns / 10) - 1,
                    "step highlight - turn count",
                    true,
                    true,
                    (int)MapEntityLayer.GUI,
                    Guid.NewGuid()));

                // ones digit.
                AddEntity(new NovaEntity(
                    point,
                    new Color(Color.White, alpha),
                    GlyphAtlas.MovementPreview0 + (turns % 10),
                    "step highlight - turn count",
                    true,
                    true,
                    (int)MapEntityLayer.GUI,
                    Guid.NewGuid()));
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
            var remainingMovement = SelectedUnit.RemainingMovement;
            var steps = path.Steps.ToList();
            var turns = remainingMovement > 0.01 ? 1 : 2;
            for (int i = 0; i < steps.Count; i++)
            {
                // TODO compute real movement cost
                var movementCost = 1;
                remainingMovement -= movementCost;
                if (remainingMovement > 0.01 && i < steps.Count - 1)
                {
                    AddEntity(new NovaEntity(
                        steps[i],
                        new Color(Color.White, 100),
                        GlyphAtlas.Solid,
                        "step highlight",
                        true,
                        true,
                        (int)MapEntityLayer.GUI,
                        Guid.NewGuid()));
                }
                else
                {
                    AddTurnCountEntities(turns, steps[i]);
                    remainingMovement = SelectedUnit.Movement;
                    turns++;
                }
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

        private void MoveSelectedUnit(Point target)
        {
            if (SelectedUnit == null)
            {
                return;
            }

            var path = AStar.ShortestPath(SelectedPoint, target);
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
