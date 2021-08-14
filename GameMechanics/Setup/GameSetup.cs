using Novalia.Maps.Generation;
using System.Collections.Generic;

namespace Novalia.GameMechanics.Setup
{
    public class GameSetup
    {
        public static GameSetup Default => new GameSetup
        {
            MapGenerationSettings = MapGenerationSettings.Default,
            PlayerEmpires = new List<Empire> { new Empire(EmpireAtlas.Sudet) },
        };

        public MapGenerationSettings MapGenerationSettings { get; set; }

        public List<Empire> PlayerEmpires { get; init; }
    }
}
