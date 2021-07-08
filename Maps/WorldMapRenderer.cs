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
        public event EventHandler<MouseScreenObjectState> RightMouseButtonDown;

        public override bool ProcessMouse(MouseScreenObjectState state)
        {
            state = new MouseScreenObjectState(this, state.Mouse);
            if (state.Mouse.RightButtonDown)
            {
                RightMouseButtonDown?.Invoke(this, state);
                return true;
            }

            if (state.Mouse.LeftClicked)
            {
                MouseClick?.Invoke(this, state);
                return true;
            }

            return base.ProcessMouse(state);
        }
    }
}
