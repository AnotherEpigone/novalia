using Novalia.Maps.Generation;

namespace Novalia
{
    public interface IGameManager
    {
        void StartNewGame(MapGenerationSettings settings);

        void Save();

        void LoadLatest();

        void Load();

        bool CanLoad();
    }
}
