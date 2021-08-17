using SadRogue.Primitives;

namespace Novalia.GameMechanics
{
    public class EmpireTemplate
    {
        public EmpireTemplate(
            string id,
            string name,
            LeaderTemplate defaultLeader,
            bool playable,
            Color color)
        {
            Id = id;
            Name = name;
            DefaultLeader = defaultLeader;
            Playable = playable;
            Color = color;
        }

        public string Id { get; }
        public string Name { get; }
        public LeaderTemplate DefaultLeader { get; }
        public bool Playable { get; }
        public Color Color { get; }
    }
}
