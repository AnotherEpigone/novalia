using SadRogue.Primitives;

namespace Novalia.Entities
{
    public interface IEntityFactory
    {
        NovaEntity CreateUnit(Point position, UnitTemplate template, Color factionColor);
    }
}