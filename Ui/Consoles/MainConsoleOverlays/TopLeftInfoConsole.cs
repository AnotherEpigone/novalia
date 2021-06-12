using SadConsole;
using SadRogue.Primitives;

namespace Novalia.Ui.Consoles.MainConsoleOverlays
{
    public class TopLeftInfoConsole : Console
    {
        public TopLeftInfoConsole(int width, int height)
            : base(width, height)
        {
            DefaultBackground = ColorHelper.ControlBack;

            Refresh();
        }

        public void Refresh()
        {
            this.Clear();
            this.Fill(background: ColorHelper.ControlBack);
            Cursor.Position = new Point(0, 0);

            var printTemplate = new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack);
            Cursor.Print(" Dalinar Kholin\r\n", printTemplate, null);
            Cursor.Print(" High King of the Alethi Empire\r\n", printTemplate, null);
            Cursor.Print(" Year: 0 | Gold: 0", printTemplate, null);
        }
    }
}
