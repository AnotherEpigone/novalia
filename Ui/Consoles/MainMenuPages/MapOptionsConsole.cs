using Novalia.Maps.Generation;
using Novalia.Ui.Controls;
using SadConsole;
using SadRogue.Primitives;
using System;
using System.Diagnostics;

namespace Novalia.Ui.Consoles.MainMenuPages
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class MapOptionsConsole : NovaControlsConsole
    {
        private readonly Rectangle _summaryBox;

        public MapOptionsConsole(int width, int height)
            : base(width, height)
        {
            Settings = new MapGenerationSettings(ContinentGeneratorStyle.Continents, 40, 40);
        }

        public MapOptionsConsole(IGameManager gameManager, int width, int height)
            : base(width, height)
        {
            Settings = MapGenerationSettings.Default;

            _summaryBox = new Rectangle(width / 2 - 55, 0, 50, 30);
            Surface.DrawBox(
                _summaryBox,
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);

            var controlsBox = new Rectangle(width / 2 + 5, 0, 50, 30);
            Surface.DrawBox(
                controlsBox,
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);

            var printTemplate = new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack);
            Cursor.Position = new Point(controlsBox.X + 21, controlsBox.Y + 2);
            Cursor.Print($"MAP SIZE", printTemplate, null);
            var tinyButton = new NovaSelectionButton(20, 1)
            {
                Text = "40 x 40",
                Position = new Point(controlsBox.X + 15, controlsBox.Y + 4),
            };
            tinyButton.Click += (_, __) =>
            {
                Settings.Width = 40;
                Settings.Height = 40;
                PrintSettings();
            };
            var mediumButton = new NovaSelectionButton(20, 1)
            {
                Text = "80 x 80",
                Position = new Point(controlsBox.X + 15, controlsBox.Y + 5),
            };
            mediumButton.Click += (_, __) =>
            {
                Settings.Width = 80;
                Settings.Height = 80;
                PrintSettings();
            };
            var largeButton = new NovaSelectionButton(20, 1)
            {
                Text = "120 x 120",
                Position = new Point(controlsBox.X + 15, controlsBox.Y + 6),
            };
            largeButton.Click += (_, __) =>
            {
                Settings.Width = 120;
                Settings.Height = 120;
                PrintSettings();
            };

            Cursor.Position = new Point(controlsBox.X + 18, controlsBox.Y + 8);
            Cursor.Print($"CONTINENT STYLE", printTemplate, null);
            var continentsButton = new NovaSelectionButton(20, 1)
            {
                Text = "Continents",
                Position = new Point(controlsBox.X + 15, controlsBox.Y + 10),
            };
            continentsButton.Click += (_, __) =>
            {
                Settings.ContinentGeneratorStyle = ContinentGeneratorStyle.Continents;
                PrintSettings();
            };
            var pangaeaButton = new NovaSelectionButton(20, 1)
            {
                Text = "Pangaea",
                Position = new Point(controlsBox.X + 15, controlsBox.Y + 11),
            };
            pangaeaButton.Click += (_, __) =>
            {
                Settings.ContinentGeneratorStyle = ContinentGeneratorStyle.Pangaea;
                PrintSettings();
            };

            var launchButton = new NovaSelectionButton(20, 1)
            {
                Text = "Launch game",
                Position = new Point(controlsBox.X + 15, controlsBox.Y + 18),
            };
            launchButton.Click += (_, __) => gameManager.StartNewGame(Settings);

            var backButton = new NovaSelectionButton(20, 1)
            {
                Text = "Back",
                Position = new Point(controlsBox.X + 15, controlsBox.Y + 20),
            };
            backButton.Click += (_, __) => Closed?.Invoke(this, EventArgs.Empty);

            PrintSettings();

            SetupSelectionButtons(
                continentsButton,
                pangaeaButton,
                tinyButton,
                mediumButton,
                largeButton,
                launchButton,
                backButton);
        }

        public event EventHandler Closed;

        public MapGenerationSettings Settings { get; }

        private void PrintSettings()
        {
            Surface.Clear(_summaryBox.Expand(-1, -1));
            var printTemplate = new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack);
            Cursor.Position = new Point(_summaryBox.X + 21, _summaryBox.Y + 2);
            Cursor.Print($"SETTINGS", printTemplate, null);
            Cursor.Position = new Point(_summaryBox.X + 2, _summaryBox.Y + 4);
            Cursor.Print($"Map size: {Settings.Width} x {Settings.Height}", printTemplate, null);
            Cursor.Position = new Point(_summaryBox.X + 2, _summaryBox.Y + 5);
            Cursor.Print($"Continent style: {Settings.ContinentGeneratorStyle}", printTemplate, null);
        }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(MapOptionsConsole)} ({Position.X}, {Position.Y})");
            }
        }
    }
}
