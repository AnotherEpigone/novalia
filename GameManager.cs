using GoRogue.MapGeneration;
using Novalia.Fonts;
using Novalia.Maps;
using Novalia.Ui;
using SadConsole;
using SadRogue.Integration;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;

namespace Novalia
{
    internal sealed class GameManager : IGameManager
    {
        private readonly IUiManager _uiManager;

        public GameManager(IUiManager uiManager)
        {
            _uiManager = uiManager;
        }

        public bool CanLoad()
        {
            return false;
        }

        public void Load()
        {
            throw new System.NotImplementedException();
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        public void StartNewGame()
        {
            var tilesetFont = Game.Instance.Fonts[_uiManager.TileFontName];
            var defaultFont = Game.Instance.DefaultFont;

            var tileSizeXFactor = (double)tilesetFont.GlyphWidth / defaultFont.GlyphWidth;
            var tileSizeYFactor = (double)tilesetFont.GlyphHeight / defaultFont.GlyphHeight;

            var tileWidth = (int)(_uiManager.ViewPortWidth / tileSizeXFactor);
            var tileHeight = (int)(_uiManager.ViewPortHeight / tileSizeYFactor);
            //var tileWidth = 10;
            //var tileHeight = 10;
            var generator = new Generator(tileWidth, tileHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(DefaultAlgorithms.RectangleMapSteps());
                });

            var generatedMap = generator.Context.GetFirst<ISettableGridView<bool>>("WallFloor");

            var map = new WorldMap(tileWidth, tileHeight, tilesetFont);

            foreach (var location in map.Positions())
            {
                bool walkable = generatedMap[location];
                int glyph = walkable ? WorldGlyphAtlas.Terrain_Grassland : WorldGlyphAtlas.Terrain_MapEdge;
                map.SetTerrain(new RogueLikeCell(location, Color.White, Color.Black, glyph, 0, walkable, walkable));
            }

            Game.Instance.Screen = _uiManager.CreateMapScreen(map);
        }
    }
}
