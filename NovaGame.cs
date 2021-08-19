using Novalia.GameMechanics;
using Novalia.GameMechanics.Combat;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Novalia
{
    public class NovaGame
    {
        public NovaGame(
            IEnumerable<Empire> empires,
            ITurnManager turnManager,
            ICombatManager combatManager)
        {
            TurnManager = turnManager;
            CombatManager = combatManager;
            Empires = empires.ToDictionary(
                e => e.Id,
                e => e);
        }

        public ITurnManager TurnManager { get; }
        public ICombatManager CombatManager { get; }
        public IReadOnlyDictionary<Guid, Empire> Empires { get; }
    }
}
