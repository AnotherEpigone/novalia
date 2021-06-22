using SadConsole;
using SadRogue.Primitives;

namespace Novalia.Ui.Consoles.MainConsoleOverlays
{
    public class EmpireStatusConsole : Console
    {
        public EmpireStatusConsole(int width, int height)
            : base(width, height)
        {
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

            var printTemplate = new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack);
            Cursor.Right(2).Print("Dalinar Kholin\r\n", printTemplate, null);
            Cursor.Right(2).Print("High King of the Alethi Empire\r\n", printTemplate, null);
            Cursor.Right(2).Print("Year: 0 Gold: 0", printTemplate, null);
        }
    }
}
