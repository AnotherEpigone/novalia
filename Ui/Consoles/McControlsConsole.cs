using Novalia.Ui.Controls;
using SadConsole.UI;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Novalia.Ui.Consoles
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class McControlsConsole : ControlsConsole
    {
        private Dictionary<McSelectionButton, System.Action> _selectionButtons;
        private McSelectionButton _lastFocusedButton;

        public McControlsConsole(int width, int height)
            : base(width, height)
        {
        }

        public void SetupSelectionButtons(params McSelectionButton[] buttons)
        {
            SetupSelectionButtons(new Dictionary<McSelectionButton, System.Action>(buttons.Select(b => new KeyValuePair<McSelectionButton, System.Action>(b, () => { }))));
        }

        public void SetupSelectionButtons(Dictionary<McSelectionButton, System.Action> buttonSelectionActions)
        {
            _selectionButtons = new Dictionary<McSelectionButton, System.Action>(buttonSelectionActions);
            if (_selectionButtons.Count < 1)
            {
                return;
            }

            var buttons = buttonSelectionActions.Keys.ToArray();
            for (int i = 1; i < _selectionButtons.Count; i++)
            {
                buttons[i - 1].NextSelection = buttons[i];
                buttons[i].PreviousSelection = buttons[i - 1];
            }

            buttons[0].PreviousSelection = buttons[_selectionButtons.Count - 1];
            buttons[_selectionButtons.Count - 1].NextSelection = buttons[0];

            foreach (var button in buttons)
            {
                Controls.Add(button);
                button.MouseEnter += (_, __) =>
                {
                    Controls.FocusedControl = button;
                };
            }

            if (buttons[0].IsEnabled)
            {
                Controls.FocusedControl = buttons[0];
            }
            else
            {
                buttons[0].SelectNext();
            }
        }

        public override void Update(System.TimeSpan time)
        {
            if (!(Controls.FocusedControl is McSelectionButton focusedButton)
                || focusedButton == _lastFocusedButton)
            {
                base.Update(time);
                return;
            }

            _lastFocusedButton = focusedButton;
            _selectionButtons[focusedButton]();

            base.Update(time);
        }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(McControlsConsole)} ({Position.X}, {Position.Y})");
            }
        }
    }
}
