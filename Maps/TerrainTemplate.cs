namespace Novalia.Maps
{
    public class TerrainTemplate
    {
        public TerrainTemplate(
            string id,
            string name,
            int glyph,
            bool walkable,
            bool transparent)
        {
            Id = id;
            Name = name;
            Glyph = glyph;
            Walkable = walkable;
            Transparent = transparent;
        }

        public string Id { get; }
        public string Name { get; }
        public int Glyph { get; }
        public bool Walkable { get; }
        public bool Transparent { get; }
    }
}
