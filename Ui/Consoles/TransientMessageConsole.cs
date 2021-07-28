using SadConsole;
using SadRogue.Primitives;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Novalia.Ui.Consoles
{
    /// <summary>
    /// Displays a single-line message that disappears after a time.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class TransientMessageConsole : SadConsole.Console
    {
        private readonly TimeSpan _hideAfter;
        private readonly object _hideTimerLock;

        private Task _hideTask;
        private CancellationTokenSource _hideTaskCancelTokenSource;

        public TransientMessageConsole(int width)
            : base(width, 1)
        {
            Surface.DefaultBackground = Color.Black.SetAlpha(100);

            IsVisible = false;

            _hideAfter = TimeSpan.FromSeconds(4);
            _hideTimerLock = new object();
        }

        public void Hide()
        {
            lock (_hideTimerLock)
            {
                if (_hideTask != null)
                {
                    _hideTaskCancelTokenSource.Cancel();
                }

                IsVisible = false;
                _hideTask = null;
            }
        }

        public void Show(string message)
        {
            IsVisible = true;
            Surface.Clear();
            Cursor.Position = new Point(0, 0);
            var coloredMessage = new ColoredString(message, Color.Gainsboro, DefaultBackground);
            Cursor.Print(coloredMessage);
            lock (_hideTimerLock)
            {
                if (_hideTask != null)
                {
                    _hideTaskCancelTokenSource.Cancel();
                }

                _hideTaskCancelTokenSource = new CancellationTokenSource();
                _hideTask = Task.Delay(_hideAfter, _hideTaskCancelTokenSource.Token)
                .ContinueWith(t =>
                {
                    if (!t.IsCanceled)
                    {
                        lock (_hideTimerLock)
                        {
                            if (!t.IsCanceled)
                            {
                                Hide();
                            }
                        }
                    }
                });
            }
        }

        private string DebuggerDisplay
        {
            get
            {
                return string.Format($"{nameof(TransientMessageConsole)} ({Position.X}, {Position.Y})");
            }
        }
    }
}
