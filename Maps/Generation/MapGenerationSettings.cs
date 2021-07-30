namespace Novalia.Maps.Generation
{
    public enum ContinentGeneratorStyle
    {
        Pangaea,
        Continents,
    }

    public class MapGenerationSettings
    {
        public MapGenerationSettings(
            ContinentGeneratorStyle continentGeneratorStyle,
            int width,
            int height)
        {
            ContinentGeneratorStyle = continentGeneratorStyle;
            Width = width;
            Height = height;
        }

        public static MapGenerationSettings Default => new MapGenerationSettings(
            ContinentGeneratorStyle.Continents,
            40,
            40);

        public ContinentGeneratorStyle ContinentGeneratorStyle { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
