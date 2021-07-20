using Novalia.Fonts;
using SadConsole;
using SadConsole.Effects;
using SadRogue.Primitives;
using SadRogue.Primitives.GridViews;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Novalia.Maps
{
    public class MinimapTerrainCellSurface : GridViewBase<ColoredGlyph>, ICellSurface
    {
        private readonly WorldMap _map;
        private readonly double _widthRatio;
        private readonly double _heightRatio;

        private bool _isDirty = true;
        private Color _defaultBackground;
        private Color _defaultForeground;

        public MinimapTerrainCellSurface(WorldMap map, int viewWidth, int viewHeight)
        {
            _map = map;
            Width = viewWidth;
            Height = viewHeight;
            _widthRatio = (double)_map.Width / (double)Width;
            _heightRatio = (double)_map.Height / (double)Height;

            Effects = new EffectsManager(this);
        }
        public override ColoredGlyph this[Point pos]
        {
            get
            {
                var referencePos = new Point(
                    (int)(pos.X * _widthRatio),
                    (int)(pos.Y * _heightRatio));
                if (_map.DefaultRenderer.Surface.View.Contains(referencePos)
                    &&(referencePos.X == _map.DefaultRenderer.Surface.View.X
                        || referencePos.X == _map.DefaultRenderer.Surface.View.MaxExtentX
                        || referencePos.Y == _map.DefaultRenderer.Surface.View.Y
                        || referencePos.Y == _map.DefaultRenderer.Surface.View.MaxExtentY))
                {
                    return new ColoredGlyph(Color.White, DefaultBackground, MinimapGlyphAtlas.Solid);
                }

                return _map.TerrainView[referencePos];
            }
        }

        public int TimesShiftedDown { get; set; }
        public int TimesShiftedRight { get; set; }
        public int TimesShiftedLeft { get; set; }
        public int TimesShiftedUp { get; set; }
        public bool UsePrintProcessor { get; set; }

        public EffectsManager Effects { get; }

        public Rectangle Area => new Rectangle(0, 0, ViewWidth, ViewHeight);

        public Color DefaultBackground
        {
            get => _defaultForeground;
            set { _defaultForeground = value; IsDirty = true; }
        }

        public Color DefaultForeground
        {
            get => _defaultBackground;
            set { _defaultBackground = value; IsDirty = true; }
        }
        public int DefaultGlyph { get; set; }
        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    IsDirtyChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public bool IsScrollable => false;

        public Rectangle View { get => Area; set => throw new NotImplementedException(); }
        public int ViewHeight { get => Height; set => throw new NotImplementedException(); }
        public int ViewWidth { get => Width; set => throw new NotImplementedException(); }
        public Point ViewPosition { get => new Point(0, 0); set => throw new NotImplementedException(); }

        public override int Height { get; }
        public override int Width { get; }

        public event EventHandler IsDirtyChanged;

        public IEnumerator<ColoredGlyph> GetEnumerator()
        {
            foreach (var pos in this.Positions())
            {
                yield return this[pos];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
