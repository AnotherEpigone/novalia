using Novalia.Ui.Controls;
using SadConsole.UI;
using System.Collections.Generic;
using System.Linq;

namespace Novalia.Ui.Windows
{
    public class NovaControlWindow : Window
    {
        public NovaControlWindow(int width, int height)
            : base(width, height) { }

        private Dictionary<NovaSelectionButton, System.Action> _selectionButtons;
        private NovaSelectionButton _lastFocusedButton;

        public void SetupSelectionButtons(params NovaSelectionButton[] buttons)
        {
            SetupSelectionButtons(new Dictionary<NovaSelectionButton, System.Action>(buttons.Select(b => new KeyValuePair<NovaSelectionButton, System.Action>(b, () => { }))));
        }

        public void SetupSelectionButtons(Dictionary<NovaSelectionButton, System.Action> buttonSelectionActions)
        {
            _selectionButtons = new Dictionary<NovaSelectionButton, System.Action>(buttonSelectionActions);
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
            if (!(Controls.FocusedControl is NovaSelectionButton focusedButton)
                || focusedButton == _lastFocusedButton)
            {
                base.Update(time);
                return;
            }

            _lastFocusedButton = focusedButton;
            _selectionButtons[focusedButton]();

            base.Update(time);
        }
    }
}
