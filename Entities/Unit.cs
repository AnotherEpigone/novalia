using SadRogue.Primitives;
using System;
using System.Diagnostics;

namespace Novalia.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Unit : NovaEntity
    {
        public Unit(
                Point position,
                int glyph,
                string name,
                bool walkable,
                bool transparent,
                int layer,
                Guid id,
                Guid empireId)
            : base(position, glyph, name, walkable, transparent, layer, id)
        {
            EmpireId = empireId;
        }

        public Guid EmpireId { get; }

        private string DebuggerDisplay => $"{nameof(Unit)}: {Name}";
    }
}
