using Newtonsoft.Json;
using Novalia.Maps;
using Novalia.Serialization.Maps;
using SadRogue.Primitives;
using System;
using System.Diagnostics;

namespace Novalia.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [JsonConverter(typeof(TerrainFeatureJsonConverter))]
    public class TerrainFeature : NovaEntity
    {

        public TerrainFeature(
            Point position,
            int glyph,
            string name,
            bool transparent,
            int movementCost)
            : base(position, glyph, name, true, transparent, (int)MapEntityLayer.TERRAINFEATURES, Guid.NewGuid())
        {
            Glyph = glyph;
            MovementCost = movementCost;
        }

        public int Glyph { get; }
        public int MovementCost { get; }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(TerrainFeature)} ({Name})");
            }
        }
    }
}
