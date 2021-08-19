using Troschuetz.Random;

namespace Novalia.GameMechanics.Combat
{
    public interface ICombatManagerFactory
    {
        ICombatManager Create(IGenerator rng);
    }
}