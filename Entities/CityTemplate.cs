namespace Novalia.Entities
{
    public class CityTemplate
    {
        public CityTemplate(
            string id,
            int glyph)
        {
            Id = id;
            Glyph = glyph;
        }

        public string Id { get; }
        public int Glyph { get; }
    }
}
