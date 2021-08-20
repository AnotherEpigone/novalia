using Novalia.Entities;
using Novalia.Maps;
using Troschuetz.Random;

namespace Novalia.GameMechanics.Combat
{
    public class CombatManager : ICombatManager
    {
        private readonly IGenerator _rng;

        public CombatManager(IGenerator rng)
        {
            _rng = rng;
        }

        public void Combat(WorldMap map, CombatContext context)
        {
            var defender = map.GetEntityAt<Unit>(context.Defender);
            var attacker = map.GetEntityAt<Unit>(context.Attacker);
            var attackerStrength = attacker.EffectiveStrength;
            var defenderStrength = defender.EffectiveStrength;

            var total = attackerStrength + defenderStrength;
            while (defender.RemainingHealth > 0 && attacker.RemainingHealth > 0)
            {
                var result = _rng.NextDouble(total);
                if (result >= defenderStrength)
                {
                    // attacker wins the round
                    defender.RemainingHealth -= 5;
                }
                else
                {
                    attacker.RemainingHealth -= 5;
                }
            }

            if (defender.RemainingHealth <= 0)
            {
                // attacker wins, defender dies
                map.RemoveEntity(defender);
                attacker.MagicMove(context.Defender);
            }
            else
            {
                map.RemoveEntity(attacker);
            }
        }
    }
}
