using Novalia.Maps;

namespace Novalia.GameMechanics.Combat
{
    public interface ICombatManager
    {
        void Combat(WorldMap map, CombatContext context);
    }
}