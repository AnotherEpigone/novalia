using Troschuetz.Random;

namespace Novalia.GameMechanics.Combat
{
    public class CombatManagerFactory : ICombatManagerFactory
    {
        public ICombatManager Create(IGenerator rng)
        {
            return new CombatManager(rng);
        }
    }
}
