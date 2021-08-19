using GoRogue.Pathing;
using Novalia.Entities;
using Novalia.Fonts;
using Novalia.GameMechanics.Combat;
using SadConsole.Input;
using SadRogue.Primitives;
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

        private Point _pathOverlayTarget;
        private bool _rmbDown;
        private bool _pathOverlayVisible;
        private Unit _selectedUnit;

        public WorldMapManager(NovaGame game, WorldMap map)
        {
            _game = game;
            _map = map;
            _map.RightMouseButtonDown += Map_RightMouseButtonDown;
            _map.LeftMouseClick += Map_LeftMouseClick;
        }

        public event EventHandler SelectionChanged;
        public event EventHandler SelectionStatsChanged;
        public event EventHandler EndTurnRequested;
        public event EventHandler<CombatContext> Combat;

        public Point SelectedPoint { get; private set; }

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

        public void OnNewturn()
        {
            foreach (var unit in _map.Entities.Items.OfType<Unit>())
            {
                unit.RemainingMovement = unit.Movement;
            }

            SelectNextUnit();
        }

        public bool HandleKeyboard(Keyboard keyboard)
        {
            var ctrl = keyboard.IsKeyDown(Keys.LeftControl) || keyboard.IsKeyDown(Keys.RightControl);
            if (keyboard.IsKeyPressed(Keys.Enter))
            {
                if (!SelectNextUnit() && _selectedUnit?.RemainingMovement < 0.01)
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

                if (SelectedUnit.RemainingMovement < 0.01)
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
                        && e.RemainingMovement > 0.01
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

        private void MoveSelectedUnit(Point target)
        {
            if (SelectedUnit == null)
            {
                return;
            }

            var path = _map.AStar.ShortestPath(SelectedPoint, target);
            if (path == null)
            {
                return;
            }

            foreach (var step in path.Steps)
            {
                var result = SelectedUnit.TryMove(step);
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
                        break;
                };

                break;
            }

            SelectedPoint = SelectedUnit.Position;
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

            var path = _map.AStar.ShortestPath(SelectedPoint, target);
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
                    _map.AddEntity(new NovaEntity(
                        steps[i],
                        new Color(Color.White, 100),
                        WorldGlyphAtlas.Solid,
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
                SelectionChanged?.Invoke(this, EventArgs.Empty);
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

            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
