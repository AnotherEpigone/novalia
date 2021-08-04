namespace Novalia.GameMechanics
{
    public class EmpireTemplate
    {
        public EmpireTemplate(
            string id,
            string name,
            LeaderTemplate defaultLeader)
        {
            Id = id;
            Name = name;
            DefaultLeader = defaultLeader;
        }

        public string Id { get; }
        public string Name { get; }
        public LeaderTemplate DefaultLeader { get; }
    }
}
