using SadConsole;
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
        { }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(WorldMap)} ({Width}, {Height})");
            }
        }
    }
}
