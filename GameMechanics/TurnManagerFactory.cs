using System.Collections.Generic;

namespace Novalia.GameMechanics
{
    public class TurnManagerFactory : ITurnManagerFactory
    {
        public ITurnManager Create(int turn, IEnumerable<Empire> empires)
        {
            return new TurnManager(turn, empires);
        }
    }
}
