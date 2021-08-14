using Novalia.Maps;
using System;

namespace Novalia.GameMechanics
{
    public interface ITurnManager
    {
        int Turn { get; }

        Empire Current { get; }

        event EventHandler NewTurn;

        void EndTurn();
        bool ReadyToEndTurn(WorldMap map);
    }
}