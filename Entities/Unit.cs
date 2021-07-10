using GoRogue.GameFramework;
using Newtonsoft.Json;
using Novalia.Fonts;
using Novalia.Maps;
using Novalia.Serialization.Entities;
using SadRogue.Primitives;
using System;
using System.Diagnostics;

namespace Novalia.Entities
{
    public enum UnitMovementResult
    {
        Moved,
        NoMovement,
        Combat,
        Blocked,
    }

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(UnitJsonConverter))]
    public class Unit : NovaEntity
    {
        private readonly NovaEntity _flag;
        private readonly NovaEntity _selectionOverlay;

        public Unit(
                Point position,
                int glyph,
                string name,
                bool walkable,
                bool transparent,
                int layer,
                Guid id,
                Guid empireId,
                Color empireColor,
                string templateId,
                float movement,
                int maxHealth)
            : base(position, glyph, name, walkable, transparent, layer, id)
        {
            EmpireId = empireId;
            EmpireColor = empireColor;
            TemplateId = templateId;
            Movement = movement;
            RemainingMovement = movement;
            MaxHealth = maxHealth;
            RemainingHealth = maxHealth;
            Selected = false;

            _flag = new NovaEntity(position, GlyphAtlas.UnitBanner, $"Unit banner {name}", true, true, (int)MapEntityLayer.EFFECTS, Guid.NewGuid());
            _flag.Appearance.Foreground = empireColor;

            _selectionOverlay = new NovaEntity(position, GlyphAtlas.SelectionOverlay, $"Selection overlay {name}", true, true, (int)MapEntityLayer.EFFECTS, Guid.NewGuid())
            {
                IsVisible = false
            };

            AddedToMap += Unit_AddedToMap;
            RemovedFromMap += Unit_RemovedFromMap;
        }

        public event EventHandler StatsChanged;

        public Guid EmpireId { get; }
        public Color EmpireColor { get; }
        public string TemplateId { get; }
        public float Movement { get; }
        public float RemainingMovement { get; set; }
        public int MaxHealth { get; }
        public float RemainingHealth { get; set; }
        public bool Selected { get; private set; }

        private string DebuggerDisplay => $"{nameof(Unit)}: {Name}";

        public void ToggleSelected()
        {
            Selected = !Selected;
            _selectionOverlay.IsVisible = Selected;
        }

        public UnitMovementResult TryMove(Point target)
        {
            if (RemainingMovement < 0.01)
            {
                return UnitMovementResult.NoMovement;
            }

            // TODO get the real movement cost
            var movementCost = 1;
            RemainingMovement = Math.Max(0, RemainingMovement - movementCost);
            StatsChanged?.Invoke(this, EventArgs.Empty);

            var oldPosition = Position;
            Position = target;
            if (Position == oldPosition)
            {
                // TODO detect combat
                return UnitMovementResult.Blocked;
            }

            _flag.Position = Position;
            _selectionOverlay.Position = Position;
            return UnitMovementResult.Moved;
        }

        private void HandleMapChange()
        {
            _flag.CurrentMap?.RemoveEntity(_flag);
            CurrentMap?.AddEntity(_flag);

            _selectionOverlay.CurrentMap?.RemoveEntity(_flag);
            CurrentMap?.AddEntity(_selectionOverlay);
        }

        private void Unit_RemovedFromMap(object sender, GameObjectCurrentMapChanged e) => HandleMapChange();

        private void Unit_AddedToMap(object sender, GameObjectCurrentMapChanged e) => HandleMapChange();
    }
}
