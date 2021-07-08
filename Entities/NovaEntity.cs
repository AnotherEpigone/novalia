using SadRogue.Integration;
using SadRogue.Primitives;
using System;
using System.Diagnostics;

namespace Novalia.Entities
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class NovaEntity : RogueLikeEntity
    {
        public NovaEntity(
            Point position,
            int glyph,
            string name,
            bool walkable,
            bool transparent,
            int layer,
            Guid id)
            :this(
                 position,
                 Color.White,
                 glyph,
                 name,
                 walkable,
                 transparent,
                 layer,
                 id)
        { }

        public NovaEntity(
            Point position,
            Color foreground,
            int glyph,
            string name,
            bool walkable,
            bool transparent,
            int layer,
            Guid id)
            : base(
                  position,
                  foreground,
                  glyph,
                  walkable: walkable,
                  transparent: transparent,
                  layer: layer)
        {
            Name = name;
            Id = id;
        }

        public Guid Id { get; }

        private string DebuggerDisplay => $"{nameof(NovaEntity)}: {Name}";
    }
}
