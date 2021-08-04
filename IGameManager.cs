using Novalia.GameMechanics.Setup;

namespace Novalia
{
    public interface IGameManager
    {
        void StartNewGame(GameSetup settings);

        void Save();

        void LoadLatest();

        void Load();

        bool CanLoad();
    }
}
