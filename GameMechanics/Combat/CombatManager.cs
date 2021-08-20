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
            // todo real strengths
            var defender = map.GetEntityAt<Unit>(context.Defender);
            var attacker = map.GetEntityAt<Unit>(context.Attacker);
            var attackerStrength = 1;
            var defenderStrength = 1;

            var total = attackerStrength + defenderStrength;
            var result = _rng.NextDouble(total);
            if (result >= defenderStrength)
            {
                // attacker wins
                map.RemoveEntity(defender);
                attacker.TryMove(context.Defender);
            }
            else
            {
                map.RemoveEntity(attacker);
            }
        }
    }
}
