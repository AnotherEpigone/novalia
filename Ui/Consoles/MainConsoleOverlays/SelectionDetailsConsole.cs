using Novalia.Maps;
using SadConsole;
using SadRogue.Primitives;

namespace Novalia.Ui.Consoles.MainConsoleOverlays
{
    public class SelectionDetailsConsole : Console
    {
        public SelectionDetailsConsole(int width, int height, WorldMap map)
            : base(width, height)
        {
            DefaultBackground = ColorHelper.ControlBack;

            UseMouse = false;

            Update(map);
        }

        public void Update(WorldMap map)
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
                Cursor.Right(2).Print("Selected unit:\r\n", printTemplate, null);
                Cursor.Right(2).Print($"{map.SelectedUnit.Name} (Empire name here)\r\n", printTemplate, null);
                Cursor.Right(2).Print($"Health: 100\r\n", printTemplate, null);
            }
        }
    }
}
