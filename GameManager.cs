using GoRogue.MapGeneration;
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
            // dummy map
            var generator = new Generator(_uiManager.ViewPortWidth, _uiManager.ViewPortHeight)
                .ConfigAndGenerateSafe(gen =>
                {
                    gen.AddSteps(DefaultAlgorithms.RectangleMapSteps());
                });

            var generatedMap = generator.Context.GetFirst<ISettableGridView<bool>>("WallFloor");

            var map = new WorldMap(_uiManager.ViewPortWidth, _uiManager.ViewPortHeight);

            foreach (var location in map.Positions())
            {
                bool walkable = generatedMap[location];
                int glyph = walkable ? '.' : '#';
                map.SetTerrain(new RogueLikeCell(location, Color.White, Color.Black, glyph, 0, walkable, walkable));
            }

            Game.Instance.Screen = _uiManager.CreateMapScreen(map);
        }
    }
}
