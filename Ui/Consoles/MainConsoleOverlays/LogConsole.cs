using SadConsole;
using SadRogue.Primitives;

namespace Novalia.Ui.Consoles.MainConsoleOverlays
{
    public class LogConsole : Console
    {
        public LogConsole(int width, int height)
            : base(width, height)
        {
            DefaultBackground = ColorHelper.ControlBack;
            UseMouse = false;

            Update();
        }

        public void Update()
        {
            this.Clear();
            this.Fill(background: ColorHelper.ControlBack);
            Surface.DrawBox(
                new Rectangle(0, 0, Width, Height),
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);
            Cursor.Position = new Point(0, 1);

            var printTemplate = new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack);
        }
    }
}
