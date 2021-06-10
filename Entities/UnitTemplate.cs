namespace Novalia.Entities
{
    public class UnitTemplate
    {
        public UnitTemplate(
            string id,
            string name,
            int glyph)
        {
            Id = id;
            Name = name;
            Glyph = glyph;
        }

        public string Id { get; }
        public string Name { get; }
        public int Glyph { get; }
    }
}
