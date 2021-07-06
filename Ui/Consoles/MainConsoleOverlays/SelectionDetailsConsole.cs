using Novalia.Maps;
using SadConsole;
using SadRogue.Primitives;

namespace Novalia.Ui.Consoles.MainConsoleOverlays
{
    public class SelectionDetailsConsole : Console
    {
        public SelectionDetailsConsole(int width, int height, WorldMap map, NovaGame game)
            : base(width, height)
        {
            DefaultBackground = ColorHelper.ControlBack;

            UseMouse = false;

            Update(map, game);
        }

        public void Update(WorldMap map, NovaGame game)
        {
            this.Clear();
            this.Fill(background: ColorHelper.ControlBack);
            Surface.DrawBox(
                new Rectangle(0, 0, Width, Height),
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);
            Cursor.Position = new Point(0, 1);

            var printTemplate = new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack);

            if (map.SelectedPoint != Point.None)
            {
                Cursor.Right(2).Print("Selected tile:\r\n", printTemplate, null);
                Cursor.Right(2).Print($"{map.SelectedPoint} {map.GetTerrainAt<Terrain>(map.SelectedPoint).Name}\r\n\r\n", printTemplate, null);
            }

            if (map.SelectedUnit != null)
            {
                var empire = game.Empires[map.SelectedUnit.EmpireId];
                Cursor.Right(2).Print("Selected unit:\r\n", printTemplate, null);
                Cursor.Right(2).Print($"{map.SelectedUnit.Name} ({empire.Name})\r\n", printTemplate, null);
                Cursor.Right(2).Print($"Health: {map.SelectedUnit.RemainingHealth}/{map.SelectedUnit.MaxHealth}\r\n", printTemplate, null);
                Cursor.Right(2).Print($"Movement: {map.SelectedUnit.RemainingMovement}/{map.SelectedUnit.Movement}\r\n", printTemplate, null);
            }
        }
    }
}
