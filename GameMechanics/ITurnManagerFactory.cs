using System.Collections.Generic;

namespace Novalia.GameMechanics
{
    public interface ITurnManagerFactory
    {
        ITurnManager Create(int turn, IEnumerable<Empire> empires);
    }
}