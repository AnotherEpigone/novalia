namespace Novalia.GameMechanics
{
    public interface ITurnManagerFactory
    {
        ITurnManager Create(int turn);
    }
}