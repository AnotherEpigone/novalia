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
                  Enum.GetNames(typeof(MapEntityLayer)).Length - 1,
                  Distance.Chebyshev,
                  entityLayersSupportingMultipleItems: GoRogue.SpatialMaps.LayerMasker.DEFAULT.Mask((int)MapEntityLayer.ITEMS, (int)MapEntityLayer.ACTORS, (int)MapEntityLayer.EFFECTS),
                  font: font)
        {
            UseMouse = true;
            base.UseMouse = false;
            Font = font;
        }

        public event EventHandler SelectionChanged;

        public Unit SelectedUnit { get; private set; }

        public Point SelectedPoint { get; private set; }

        public override bool UseMouse { get; set; }

        private string DebuggerDisplay => string.Format($"{nameof(WorldMap)} ({Width}, {Height})");

        public IFont Font { get; }

        protected override bool ProcessMouse(MouseScreenObjectState state)
        {
            state = new MouseScreenObjectState(BackingObject, state.Mouse.Clone());
            if (state.Mouse.LeftClicked)
            {
                SelectedPoint = state.CellPosition;
                SelectedUnit?.ToggleSelected();
                SelectedUnit = null;

                var clickedUnit = GetEntityAt<Unit>(state.CellPosition);
                if (clickedUnit != null)
                {
                    clickedUnit.ToggleSelected();
                    SelectedUnit = clickedUnit;
                }

                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }

            return base.ProcessMouse(state);
        }
    }
}
