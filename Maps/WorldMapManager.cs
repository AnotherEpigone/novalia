using GoRogue.GameFramework;
using GoRogue.Pathing;
using Novalia.Entities;
using Novalia.Fonts;
using Novalia.GameMechanics.Combat;
using SadConsole.Input;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using System;
using System.Linq;

namespace Novalia.Maps
{
    /// <summary>
    /// Handles input and movement logic for a world map
    /// </summary>
    public class WorldMapManager
    {
        private readonly NovaGame _game;
        private readonly WorldMap _map;
        private readonly AStar _aStar;

        private Point _pathOverlayTarget;
        private bool _rmbDown;
        private bool _pathOverlayVisible;
        private Unit _selectedUnit;
        private Point _selectedPoint;

        public WorldMapManager(NovaGame game, WorldMap map)
        {
            _game = game;
            _map = map;
            _map.RightMouseButtonDown += Map_RightMouseButtonDown;
            _map.LeftMouseClick += Map_LeftMouseClick;

            _aStar = new AStar(
                _map.WalkabilityView,
                Distance.Chebyshev,
                new LambdaGridView<double>(
                    _map.Width,
                    _map.Height,
                    p => (double)GetMovementCost(p) + 1d),
                    1d);
        }

        public event EventHandler SelectionChanged;
        public event EventHandler SelectionStatsChanged;
        public event EventHandler EndTurnRequested;
        public event EventHandler<CombatContext> Combat;

        public Point SelectedPoint
        {
            get => _selectedPoint;
            private set
            {
                _selectedPoint = value;
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Unit SelectedUnit
        {
            get => _selectedUnit;
            private set
            {
                if (_selectedUnit != null)
                {
                    _selectedUnit.StatsChanged -= SelectionStatsChanged;
                    _selectedUnit.RemovedFromMap -= SelectedUnit_RemovedFromMap;
                }

                _selectedUnit = value;
                if (_selectedUnit != null)
                {
                    _selectedUnit.StatsChanged += SelectionStatsChanged;
                    _selectedUnit.RemovedFromMap += SelectedUnit_RemovedFromMap;
                }

                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void OnNewTurn()
        {
            if (_game.TurnManager.Current.Playable)
            {
                SelectNextUnit();
            }
        }

        public void OnNewRound()
        {
            foreach (var unit in _map.Entities.Items.OfType<Unit>())
            {
                unit.RemainingMovement = unit.Movement;
            }
        }

        public bool HandleKeyboard(Keyboard keyboard)
        {
            var ctrl = keyboard.IsKeyDown(Keys.LeftControl) || keyboard.IsKeyDown(Keys.RightControl);
            if (keyboard.IsKeyPressed(Keys.Enter))
            {
                if (!SelectNextUnit() && !(_selectedUnit?.HasMovement() ?? false))
                {
                    EndTurnRequested?.Invoke(this, EventArgs.Empty);
                }

                return true;
            }

            if (ctrl && keyboard.IsKeyPressed(Keys.E))
            {
                EndTurnRequested?.Invoke(this, EventArgs.Empty);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.C))
            {
                _map.DefaultRenderer.Surface.View = _map.DefaultRenderer.Surface.View.WithCenter(SelectedPoint);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Down))
            {
                _map.DefaultRenderer.Surface.View = _map.DefaultRenderer.Surface.View.ChangePosition(Direction.Down);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Up))
            {
                _map.DefaultRenderer.Surface.View = _map.DefaultRenderer.Surface.View.ChangePosition(Direction.Up);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Right))
            {
                _map.DefaultRenderer.Surface.View = _map.DefaultRenderer.Surface.View.ChangePosition(Direction.Right);
                return true;
            }

            if (keyboard.IsKeyPressed(Keys.Left))
            {
                _map.DefaultRenderer.Surface.View = _map.DefaultRenderer.Surface.View.ChangePosition(Direction.Left);
                return true;
            }

            return false;
        }

        public void Update()
        {
            if (_pathOverlayVisible && !_rmbDown)
            {
                MoveSelectedUnit(_pathOverlayTarget);
                ClearPathOverlay();

                if (SelectedUnit == null || !SelectedUnit.HasMovement())
                {
                    SelectNextUnit();
                }
            }

            _rmbDown = false;
        }

        public bool SelectNextUnit()
        {
            var moveableUnit = _map.Entities.Items
                    .OfType<Unit>()
                    .Where(e => e.EmpireId == _game.TurnManager.Current.Id
                        && e.HasMovement()
                        && e != SelectedUnit)
                    .OrderBy(u => u.LastSelected)
                    .FirstOrDefault();
            if (moveableUnit == null)
            {
                return false;
            }

            SelectedUnit?.ToggleSelected();
            SelectedUnit = moveableUnit;
            SelectedPoint = SelectedUnit.Position;
            SelectedUnit.ToggleSelected();
            _map.DefaultRenderer.Surface.View = _map.DefaultRenderer.Surface.View.WithCenter(SelectedPoint);

            return true;
        }

        private void ClearPathOverlay()
        {
            _pathOverlayVisible = false;
            _pathOverlayTarget = Point.None;
            ClearGui();
        }

        private void ClearGui()
        {
            var overlayEntities = _map.Entities.GetLayer((int)MapEntityLayer.GUI).Items;
            foreach (var entity in overlayEntities)
            {
                _map.RemoveEntity(entity);
            }
        }

        private void AddTurnCountEntities(int turns, Point point)
        {
            var alpha = turns > 1
                        ? 150
                        : 210;
            if (turns < 10)
            {
                _map.AddEntity(new NovaEntity(
                    point,
                    new Color(Color.White, alpha),
                    WorldGlyphAtlas.MovementPreview1 + turns - 1,
                    "step highlight - turn count",
                    true,
                    true,
                    (int)MapEntityLayer.GUI,
                    Guid.NewGuid()));
            }
            else if (turns > 99)
            {
                _map.AddEntity(new NovaEntity(
                    point,
                    new Color(Color.White, alpha),
                    WorldGlyphAtlas.MovementPreview99plus,
                    "step highlight - turn count",
                    true,
                    true,
                    (int)MapEntityLayer.GUI,
                    Guid.NewGuid()));
            }
            else
            {
                // tens digit.
                _map.AddEntity(new NovaEntity(
                    point,
                    new Color(Color.White, alpha),
                    WorldGlyphAtlas.MovementPreview10 + (turns / 10) - 1,
                    "step highlight - turn count",
                    true,
                    true,
                    (int)MapEntityLayer.GUI,
                    Guid.NewGuid()));

                // ones digit.
                _map.AddEntity(new NovaEntity(
                    point,
                    new Color(Color.White, alpha),
                    WorldGlyphAtlas.MovementPreview0 + (turns % 10),
                    "step highlight - turn count",
                    true,
                    true,
                    (int)MapEntityLayer.GUI,
                    Guid.NewGuid()));
            }
        }

        private int GetMovementCost(Point target)
        {
            var cost = 1;
            var feature = _map.GetEntityAt<TerrainFeature>(target);
            if (feature != null)
            {
                cost += feature.MovementCost;
            }

            return cost;
        }

        private void MoveSelectedUnit(Point target)
        {
            if (SelectedUnit == null)
            {
                return;
            }

            var path = _aStar.ShortestPath(SelectedPoint, target);
            if (path == null)
            {
                return;
            }

            foreach (var step in path.Steps)
            {
                var movementCost = GetMovementCost(step);
                var result = SelectedUnit.TryMove(step, movementCost);
                switch (result)
                {
                    case UnitMovementResult.NoMovement:
                    case UnitMovementResult.Blocked:
                        break;
                    case UnitMovementResult.Moved:
                        continue;
                    case UnitMovementResult.Combat:
                        Combat?.Invoke(
                            this,
                            new CombatContext(SelectedUnit.Position, target));
                        if (SelectedUnit == null || !SelectedUnit.HasMovement())
                        {
                            SelectNextUnit();
                        }

                        break;
                };

                break;
            }

            SelectedPoint = SelectedUnit?.Position ?? Point.None;
        }

        private void Map_RightMouseButtonDown(object sender, MouseScreenObjectState e)
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

            if (!_map.Terrain[target].IsWalkable)
            {
                _rmbDown = true;
                return;
            }

            var path = _aStar.ShortestPath(SelectedPoint, target);
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
                var movementCost = GetMovementCost(steps[i]);
                remainingMovement -= movementCost;
                if (remainingMovement > 0.01 && i < steps.Count - 1)
                {
                    var alpha = turns > 1
                        ? 150
                        : 210;
                    _map.AddEntity(new NovaEntity(
                        steps[i],
                        new Color(Color.White, alpha),
                        WorldGlyphAtlas.MovementPreviewDot,
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

        private void Map_LeftMouseClick(object sender, MouseScreenObjectState e)
        {
            SelectedUnit?.ToggleSelected();
            SelectedUnit = null;
            if (SelectedPoint == e.CellPosition)
            {
                SelectedPoint = Point.None;
                return;
            }

            SelectedPoint = e.CellPosition;

            var clickedUnit = _map.GetEntityAt<Unit>(e.CellPosition);
            if (clickedUnit != null
                && clickedUnit.EmpireId == _game.TurnManager.Current.Id)
            {
                clickedUnit.ToggleSelected();
                SelectedUnit = clickedUnit;
            }
        }

        private void SelectedUnit_RemovedFromMap(object sender, GameObjectCurrentMapChanged e)
        {
            SelectedUnit = null;
            SelectNextUnit();
        }
    }
}
