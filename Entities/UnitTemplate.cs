namespace Novalia.Entities
{
    public class UnitTemplate
    {
        public UnitTemplate(
            string id,
            string name,
            int glyph,
            float movement,
            int maxHealth,
            int strength)
        {
            Id = id;
            Name = name;
            Glyph = glyph;
            Movement = movement;
            MaxHealth = maxHealth;
            Strength = strength;
        }

        public string Id { get; }
        public string Name { get; }
        public int Glyph { get; }
        public float Movement { get; }
        public int MaxHealth { get; }
        public int Strength { get; }
    }
}
