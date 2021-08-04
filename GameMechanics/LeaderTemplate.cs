namespace Novalia.GameMechanics
{
    public class LeaderTemplate
    {
        public LeaderTemplate(
            string id,
            string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }
    }
}
