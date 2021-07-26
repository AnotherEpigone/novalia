using Novalia.Maps;
using System;

namespace Novalia.GameMechanics
{
    public interface ITurnManager
    {
        int Turn { get; }

        event EventHandler NewTurn;

        void EndTurn();
        bool ReadyToEndTurn(WorldMap map, Guid playerEmpireId);
    }
}