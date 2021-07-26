using Novalia.GameMechanics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Novalia
{
    public class NovaGame
    {
        public NovaGame(
            Guid playerEmpireId,
            Empire[] empires,
            ITurnManager turnManager)
        {
            PlayerEmpireId = playerEmpireId;
            TurnManager = turnManager;
            Empires = empires.ToDictionary(
                e => e.Id,
                e => e);
        }

        public Guid PlayerEmpireId { get; }
        public ITurnManager TurnManager { get; }
        public IReadOnlyDictionary<Guid, Empire> Empires { get; }
    }
}
