namespace Novalia.Entities
{
    public class TerrainFeatureTemplate
    {
        public TerrainFeatureTemplate(
            string id,
            int glyph,
            string name,
            bool transparent)
        {
            Id = id;
            Glyph = glyph;
            Name = name;
            Transparent = transparent;
        }

        public string Id { get; }
        public int Glyph { get; }
        public string Name { get; }
        public bool Transparent { get; }
    }
}
