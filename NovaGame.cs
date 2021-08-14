using Novalia.GameMechanics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Novalia
{
    public class NovaGame
    {
        public NovaGame(
            IEnumerable<Empire> empires,
            ITurnManager turnManager)
        {
            TurnManager = turnManager;
            Empires = empires.ToDictionary(
                e => e.Id,
                e => e);
        }

        public ITurnManager TurnManager { get; }
        public IReadOnlyDictionary<Guid, Empire> Empires { get; }
    }
}
