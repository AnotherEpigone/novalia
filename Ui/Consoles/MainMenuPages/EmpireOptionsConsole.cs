using Novalia.GameMechanics;
using Novalia.GameMechanics.Setup;
using Novalia.Ui.Controls;
using SadConsole;
using SadConsole.UI.Controls;
using SadRogue.Primitives;
using System;
using System.Diagnostics;
using System.Linq;

namespace Novalia.Ui.Consoles.MainMenuPages
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class EmpireOptionsConsole : NovaControlsConsole, IGameSetupStage
    {
        private const int MaxPlayers = 2;
        private readonly Rectangle _controlBox;
        private readonly NovaSelectionButton _newPlayerButton;
        private readonly NovaSelectionButton _nextButton;
        private readonly NovaSelectionButton _backButton;
        private int _numberOfPlayers;

        public EmpireOptionsConsole(GameSetup gameSetup, int width, int height)
            : base(width, height)
        {
            GameSetup = gameSetup;
            _numberOfPlayers = 1;

            _controlBox = new Rectangle(width / 2 + 40, 0, 30, 15);

            _newPlayerButton = new NovaSelectionButton(20, 1)
            {
                IsEnabled = _numberOfPlayers < MaxPlayers,
                Text = "Add player",
                Position = new Point(_controlBox.X + 5, _controlBox.Y + 5),
            };
            _newPlayerButton.Click += (_, __) =>
            {
                _numberOfPlayers++;
                _newPlayerButton.IsEnabled = _numberOfPlayers < MaxPlayers;
                Refresh();
            };

            _nextButton = new NovaSelectionButton(20, 1)
            {
                Text = "Start game",
                Position = new Point(_controlBox.X + 5, _controlBox.Y + 7),
            };
            _nextButton.Click += (_, __) => Next?.Invoke(this, EventArgs.Empty);

            _backButton = new NovaSelectionButton(20, 1)
            {
                Text = "Back",
                Position = new Point(_controlBox.X + 5, _controlBox.Y + 9),
            };
            _backButton.Click += (_, __) => Closed?.Invoke(this, EventArgs.Empty);

            Refresh();
        }

        public GameSetup GameSetup { get; }

        public event EventHandler Closed;

        public event EventHandler Next;

        private void Refresh()
        {
            Surface.Clear();
            Controls.Clear();

            for (int i = 0; i< _numberOfPlayers; i++)
            {
                var playerBox = new Rectangle(Width / 2 - 70, i * 15, 105, 15);
                PrintPlayerBox(playerBox, i);
            }

            Surface.DrawBox(
                _controlBox,
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);

            SetupSelectionButtons(
                _newPlayerButton,
                _nextButton,
                _backButton);
        }

        private void PrintPlayerBox(Rectangle playerBox, int playerIndex)
        {
            Surface.DrawBox(
                playerBox,
                new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack),
                connectedLineStyle: ICellSurface.ConnectedLineThin);

            Surface.Clear(playerBox.Expand(-1, -1));
            var printTemplate = new ColoredGlyph(ColorHelper.Text, ColorHelper.ControlBack);
            Cursor.Position = new Point(playerBox.X + 2, playerBox.Y + 1);
            Cursor.Print($"Leader: ", printTemplate, null);
            Cursor.Position = new Point(playerBox.X + 2, playerBox.Y + 3);
            Cursor.Print($"Empire:", printTemplate, null);

            var leaderTextBox = new TextBox(20)
            {
                Text = GameSetup.PlayerEmpire.Leader.Name,
                Position = new Point(playerBox.X + 10, playerBox.Y + 1),
            };
            leaderTextBox.TextChanged += (_, __) => GameSetup.PlayerEmpire.Leader.Name = leaderTextBox.Text;

            var playableEmpires = EmpireAtlas.ById.Values
                .Where(e => e.Playable);
            var selectionBox = new ListBox(20, 10)
            {
                Position = new Point(playerBox.X + 2, playerBox.Y + 4),
            };
            ((SadConsole.UI.Themes.ListBoxTheme)selectionBox.Theme).DrawBorder = true;
            selectionBox.SelectedItemChanged += (_, e) =>
            {
                GameSetup.PlayerEmpire = new Empire(EmpireAtlas.ByName[(string)e.Item]);
                leaderTextBox.Text = GameSetup.PlayerEmpire.Leader.Name;
            };
            foreach (var empire in playableEmpires)
            {
                selectionBox.Items.Add(empire.Name);
            }

            Controls.Add(selectionBox);
            Controls.Add(leaderTextBox);
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
