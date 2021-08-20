using Novalia.Maps;
using System;

namespace Novalia.GameMechanics
{
    public interface ITurnManager
    {
        int Round { get; }

        Empire Current { get; }

        event EventHandler NewTurn;
        event EventHandler NewRound;

        void EndTurn();
        bool ReadyToEndTurn(WorldMap map);
    }
}