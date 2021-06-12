namespace Novalia
{
    public interface IGameManager
    {
        void StartNewGame();

        void Save();

        void LoadLatest();

        void Load();

        bool CanLoad();
    }
}
