using Novalia.Maps.Generation;

namespace Novalia.GameMechanics.Setup
{
    public class GameSetup
    {
        public static GameSetup Default => new GameSetup
        {
            MapGenerationSettings = MapGenerationSettings.Default,
            PlayerEmpire = new Empire(EmpireAtlas.Sudet),
        };

        public MapGenerationSettings MapGenerationSettings { get; set; }

        public Empire PlayerEmpire { get; set; }
    }
}
