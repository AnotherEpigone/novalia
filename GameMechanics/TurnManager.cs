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

        public TurnManager(int round, IEnumerable<Empire> empires)
        {
            Round = round;
            _empires = empires.ToList();

            _currentEmpireIndex = 0;
        }

        public event EventHandler NewTurn;
        public event EventHandler NewRound;

        public int Round { get; private set; }

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
                Round++;
                NewRound?.Invoke(this, EventArgs.Empty);
            }

            NewTurn?.Invoke(this, EventArgs.Empty);
        }
    }
}
