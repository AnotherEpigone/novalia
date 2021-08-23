using Novalia.Entities;
using Novalia.Maps;
using SadConsole;
using SadRogue.Primitives;

namespace Novalia.Ui.Consoles.MainConsoleOverlays
{
    public class SelectionDetailsConsole : Console
    {
        public SelectionDetailsConsole(int width, int height, WorldMapManager mapManager, WorldMap map, NovaGame game)
            : base(width, height)
        {
            DefaultBackground = ColorHelper.ControlBack;

            UseMouse = false;

            Update(mapManager, map, game);
        }

        public void Update(WorldMapManager mapManager, WorldMap map, NovaGame game)
        {
            this.Clear();
            this.Fill(background: ColorHelper.ControlBack);
            Surface.DrawBox(
                new Rectangle(0, 0, Width, Height),
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);
            Cursor.Position = new Point(0, 1);

            var printTemplate = new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack);

            if (mapManager.SelectedPoint != Point.None)
            {
                Cursor.Right(2).Print("Selected tile:\r\n", printTemplate, null);
                Cursor.Right(2).Print($"{mapManager.SelectedPoint} {map.GetTerrainAt<Terrain>(mapManager.SelectedPoint).Name}\r\n\r\n", printTemplate, null);
            }

            if (mapManager.SelectedUnit != null)
            {
                var empire = game.Empires[mapManager.SelectedUnit.EmpireId];
                Cursor.Right(2).Print("Selected unit:\r\n", printTemplate, null);
                Cursor.Right(2).Print($"{mapManager.SelectedUnit.Name} ({empire.Name})\r\n", printTemplate, null);
                Cursor.Right(2).Print($"Health: {mapManager.SelectedUnit.RemainingHealth}/{mapManager.SelectedUnit.MaxHealth}", printTemplate, null);
                Cursor.Right(2).Print($"Movement: {mapManager.SelectedUnit.RemainingMovement}/{mapManager.SelectedUnit.Movement}\r\n", printTemplate, null);
                Cursor.Right(2).Print($"Strength: {mapManager.SelectedUnit.Strength}", printTemplate, null);
                Cursor.Right(2).Print($"Effective: {mapManager.SelectedUnit.EffectiveStrength}\r\n", printTemplate, null);
            }
            else if (mapManager.SelectedPoint != Point.None)
            {
                var unit = map.GetEntityAt<Unit>(mapManager.SelectedPoint);
                if (unit != null)
                {
                    var empire = game.Empires[unit.EmpireId];
                    Cursor.Right(2).Print($"{unit.Name} ({empire.Name})\r\n", printTemplate, null);
                    Cursor.Right(2).Print($"Health: {unit.RemainingHealth}/{unit.MaxHealth}", printTemplate, null);
                    Cursor.Right(2).Print($"Movement: {unit.RemainingMovement}/{unit.Movement}\r\n", printTemplate, null);
                    Cursor.Right(2).Print($"Strength: {unit.Strength}", printTemplate, null);
                    Cursor.Right(2).Print($"Effective: {unit.EffectiveStrength}\r\n", printTemplate, null);
                }
            }
        }
    }
}
