using SadConsole;
using SadRogue.Primitives;

namespace Novalia.Ui.Consoles.MainConsoleOverlays
{
    public class EmpireStatusConsole : Console
    {
        private readonly NovaGame _game;

        public EmpireStatusConsole(int width, int height, NovaGame game)
            : base(width, height)
        {
            _game = game;
            _game.TurnManager.NewTurn += (_, __) => Refresh();

            DefaultBackground = ColorHelper.ControlBack;

            UseMouse = false;

            Refresh();
        }

        public void Refresh()
        {
            this.Clear();
            this.Fill(background: ColorHelper.ControlBack);
            Surface.DrawBox(
                new Rectangle(0, 0, Width, Height),
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);
            Cursor.Position = new Point(0, 1);

            var playerEmpire = _game.TurnManager.Current;
            var printTemplate = new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack);
            Cursor.Right(2).Print($"{playerEmpire.Leader.Name}\r\n", printTemplate, null);
            Cursor.Right(2).Print($"{playerEmpire.Name}\r\n", printTemplate, null);
            Cursor.Right(2).Print($"Year: {_game.TurnManager.Round} Gold: 0", printTemplate, null);
        }
    }
}
