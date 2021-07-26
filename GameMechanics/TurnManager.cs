using Novalia.Entities;
using Novalia.Maps;
using System;
using System.Linq;

namespace Novalia.GameMechanics
{
    public class TurnManager : ITurnManager
    {
        public TurnManager()
        {
            Turn = 0;
        }

        public event EventHandler NewTurn;

        public int Turn { get; private set; }

        public void InitTurn(int turn) => Turn = turn;

        public bool ReadyToEndTurn(WorldMap map, Guid playerEmpireId)
        {
            return !map.Entities.Items
                .OfType<Unit>()
                .Any(e => e.EmpireId == playerEmpireId
                        && e.RemainingMovement > 0.01);
        }

        public void EndTurn()
        {
            Turn++;
            NewTurn?.Invoke(this, EventArgs.Empty);
        }
    }
}
