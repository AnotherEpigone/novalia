using Novalia.GameMechanics.Setup;
using Novalia.Ui.Controls;
using SadConsole;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using System;
using System.Diagnostics;

namespace Novalia.Ui.Consoles.MainMenuPages
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class EmpireOptionsConsole : NovaControlsConsole, IGameSetupStage
    {
        private readonly Rectangle _playerBox;
        private readonly Rectangle _controlBox;

        public EmpireOptionsConsole(GameSetup gameSetup, int width, int height)
            : base(width, height)
        {
            GameSetup = gameSetup;

            _playerBox = new Rectangle(width / 2 - 70, 0, 105, 15);
            Surface.DrawBox(
                _playerBox,
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);

            _controlBox = new Rectangle(width / 2 + 40, 0, 30, 15);
            Surface.DrawBox(
                _controlBox,
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);

            var leaderTextBox = new TextBox(20)
            {
                Text = gameSetup.PlayerEmpire.Leader.Name,
                Position = new Point(_playerBox.X + 10, _playerBox.Y + 1),
            };
            leaderTextBox.TextChanged += (_, __) => GameSetup.PlayerEmpire.Leader.Name = leaderTextBox.Text;

            var newPlayerButton = new NovaSelectionButton(20, 1)
            {
                IsEnabled = false,
                Text = "Add player",
                Position = new Point(_controlBox.X + 5, _controlBox.Y + 5),
            };

            var nextButton = new NovaSelectionButton(20, 1)
            {
                Text = "Start game",
                Position = new Point(_controlBox.X + 5, _controlBox.Y + 7),
            };
            nextButton.Click += (_, __) => Next?.Invoke(this, EventArgs.Empty);

            var backButton = new NovaSelectionButton(20, 1)
            {
                Text = "Back",
                Position = new Point(_controlBox.X + 5, _controlBox.Y + 9),
            };
            backButton.Click += (_, __) => Closed?.Invoke(this, EventArgs.Empty);

            PrintEmpires();

            SetupSelectionButtons(
                newPlayerButton,
                nextButton,
                backButton);

            Controls.Add(leaderTextBox);
        }

        public GameSetup GameSetup { get; }

        public event EventHandler Closed;

        public event EventHandler Next;

        private void PrintEmpires()
        {
            Surface.Clear(_playerBox.Expand(-1, -1));
            var printTemplate = new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack);
            Cursor.Position = new Point(_playerBox.X + 2, _playerBox.Y + 1);
            Cursor.Print($"Leader: ", printTemplate, null);
            Cursor.Position = new Point(_playerBox.X + 2, _playerBox.Y + 3);
            Cursor.Print($"Empire: {GameSetup.PlayerEmpire.Name}", printTemplate, null);
        }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(EmpireOptionsConsole)} ({Position.X}, {Position.Y})");
            }
        }
    }
}
