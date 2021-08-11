namespace Novalia.GameMechanics
{
    public class EmpireTemplate
    {
        public EmpireTemplate(
            string id,
            string name,
            LeaderTemplate defaultLeader,
            bool playable)
        {
            Id = id;
            Name = name;
            DefaultLeader = defaultLeader;
            Playable = playable;
        }

        public string Id { get; }
        public string Name { get; }
        public LeaderTemplate DefaultLeader { get; }
        public bool Playable { get; }
    }
}
