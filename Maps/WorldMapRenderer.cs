using SadConsole;
using SadConsole.Input;
using SadRogue.Primitives;
using System;

namespace Novalia.Maps
{
    public class WorldMapRenderer : ScreenSurface
    {
        public WorldMapRenderer(
            ICellSurface surface,
            IFont font,
            Point fontSize)
            : base(surface, font, fontSize)
        {
        }

        public event EventHandler<MouseScreenObjectState> MouseClick;

        public override bool ProcessMouse(MouseScreenObjectState state)
        {
            if (state.Mouse.LeftClicked)
            {
                MouseClick.Invoke(this, state);
            }

            return base.ProcessMouse(state);
        }
    }
}
