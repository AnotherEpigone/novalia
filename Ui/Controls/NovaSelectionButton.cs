using SadConsole.Input;
using SadConsole.UI.Controls;
using System.Collections.Generic;

namespace Novalia.Ui.Controls
{
    /// <summary>
    /// Provides a button-like control that changes focus to a designated previous or next selection button when the arrow keys are pushed.
    /// </summary>
    public class NovaSelectionButton : Button
    {
        /// <summary>
        /// The selection button to focus when the UP key is pressed or the SelectPrevious() method is called.
        /// </summary>
        public NovaSelectionButton PreviousSelection { get; set; }

        /// <summary>
        /// The selection button to focus when the DOWN key is pressed or the SelectNext() method is called.
        /// </summary>
        public NovaSelectionButton NextSelection { get; set; }


        /// <summary>
        /// Creates a new Selection Button with a specific width and height.
        /// </summary>
        /// <param name="width">The width of the selection button.</param>
        public NovaSelectionButton(int width, int height) : base(width, height)
        {
        }

        /// <summary>
        /// Sets the next selection button and optionally sets the previous of the referenced selection to this button.
        /// </summary>
        /// <param name="nextSelection">The selection button to be used as next.</param>
        /// <param name="setPreviousOnNext">Sets the PreviousSelection property on the <paramref name="nextSelection"/> instance to current selection button. Defaults to true.</param>
        /// <returns></returns>
        public NovaSelectionButton SetNextSelection(ref NovaSelectionButton nextSelection, bool setPreviousOnNext = true)
        {
            NextSelection = nextSelection;

            if (setPreviousOnNext)
            {
                nextSelection.PreviousSelection = this;
            }

            return nextSelection;
        }

        /// <summary>
        /// Focuses the previous or next selection button depending on if the UP or DOWN arrow keys were pressed.
        /// </summary>
        /// <param name="info">The keyboard state.</param>
        public override bool ProcessKeyboard(Keyboard info)
        {
            if (info.IsKeyReleased(Keys.Up))
            {
                SelectPrevious();
                return true;
            }
            
            if (info.IsKeyReleased(Keys.Down))
            {
                SelectNext();
                return true;
            }

            return base.ProcessKeyboard(info);
        }

        /// <summary>
        /// Selects the previous selection button.
        /// </summary>
        /// <returns>Returns the previous selection button.</returns>
        public NovaSelectionButton SelectPrevious()
        {
            if (PreviousSelection == null || PreviousSelection == this)
            {
                return null;
            }

            if (!PreviousSelection.IsEnabled)
            {
                return PreviousSelection.SelectPrevious();
            }

            PreviousSelection.IsFocused = true;
            return PreviousSelection;
        }

        /// <summary>
        /// Selects the next selection button.
        /// </summary>
        /// <returns>Returns the next selection button.</returns>
        public NovaSelectionButton SelectNext()
        {
            if (NextSelection == null || NextSelection == this)
            {
                return null;
            }

            if (!NextSelection.IsEnabled)
            {
                // scanning for the next button like this will stack overflow if it loops,
                // so we maintain the stack here.
                // Note, we don't include this button yet. Looping back to self is acceptable.
                return NextSelection.SelectNextProtected(new HashSet<NovaSelectionButton>());
            }

            NextSelection.IsFocused = true;
            return NextSelection;
        }

        private NovaSelectionButton SelectNextProtected(HashSet<NovaSelectionButton> stack)
        {
            if (stack.Contains(this)
                || NextSelection == null
                || NextSelection == this)
            {
                // Either no next set, or we're in a loop with no enabled buttons
                return null;
            }

            stack.Add(this);

            if (!NextSelection.IsEnabled)
            {
                return NextSelection.SelectNextProtected(stack);
            }

            NextSelection.IsFocused = true;
            return NextSelection;
        }
    }
}
