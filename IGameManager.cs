namespace Novalia
{
    public interface IGameManager
    {
        void StartNewGame();

        void Save();

        void Load();

        bool CanLoad();
    }
}
