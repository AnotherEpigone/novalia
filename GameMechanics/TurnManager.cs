using Novalia.Entities;
using Novalia.Maps;
using System;
using System.Linq;

namespace Novalia.GameMechanics
{
    public class TurnManager
    {
        public TurnManager(int turn)
        {
            Turn = turn;
        }

        public int Turn { get; }

        public bool ReadyToEndTurn(WorldMap map, Guid playerEmpireId)
        {
            return !map.Entities.Items
                .OfType<Unit>()
                .Any(e => e.EmpireId == playerEmpireId
                        && e.RemainingMovement > 0.01);
        }
    }
}
