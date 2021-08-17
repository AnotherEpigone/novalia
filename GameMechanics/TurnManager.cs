using Novalia.Entities;
using Novalia.Maps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Novalia.GameMechanics
{
    public class TurnManager : ITurnManager
    {
        private readonly List<Empire> _empires;
        private int _currentEmpireIndex;

        public TurnManager(int turn, IEnumerable<Empire> empires)
        {
            Turn = turn;
            _empires = empires.ToList();

            _currentEmpireIndex = 0;
        }

        public event EventHandler NewTurn;

        public int Turn { get; private set; }

        public Empire Current => _empires[_currentEmpireIndex];

        public bool ReadyToEndTurn(WorldMap map)
        {
            return !map.Entities.Items
                .OfType<Unit>()
                .Any(e => e.EmpireId == Current.Id
                        && e.RemainingMovement > 0.01);
        }

        public void EndTurn()
        {
            _currentEmpireIndex = ++_currentEmpireIndex % _empires.Count;
            if(_currentEmpireIndex == 0)
            {
                Turn++;
            }

            NewTurn?.Invoke(this, EventArgs.Empty);
        }
    }
}
