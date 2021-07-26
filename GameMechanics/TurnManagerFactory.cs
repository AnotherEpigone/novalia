namespace Novalia.GameMechanics
{
    public class TurnManagerFactory : ITurnManagerFactory
    {
        public ITurnManager Create(int turn)
        {
            return new TurnManager(turn);
        }
    }
}
