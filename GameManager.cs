namespace Novalia
{
    internal sealed class GameManager : IGameManager
    {
        public bool CanLoad()
        {
            return false;
        }

        public void Load()
        {
            throw new System.NotImplementedException();
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        public void StartNewGame()
        {
            throw new System.NotImplementedException();
        }
    }
}
