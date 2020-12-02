using System;

namespace Launcher
{
    /// <summary>
    /// The input handler provides input functionality.
    /// if ListenForInputs() is run in a while loop, any key press can be logged and trigger an action through the event
    /// </summary>
    public class InputHandler
    {        
        private InputEventArgs _inputArgs = new InputEventArgs();

        public EventHandler<InputEventArgs> InputReceived;

        protected virtual void OnInput(InputEventArgs inputArgs)
        {
            InputReceived?.Invoke(this, inputArgs);
        }

        public void ListenForInputs()
        {
            if (Console.KeyAvailable)
            {
                _inputArgs.Key = Console.ReadKey(true).Key;
                OnInput(_inputArgs);
            }
        }
    }

    /// <summary>
    /// The essential inputs required by the game.
    /// Vertical is used to navigate the menu
    /// Vertical and Horizontal are used to move the player character
    /// </summary>
    public class InputEventArgs : EventArgs
    {
        public ConsoleKey Key { get; set; }
        public int Horizontal { get { return (Key == ConsoleKey.LeftArrow) ? -1 : (Key == ConsoleKey.RightArrow) ? 1 : 0; } }
        public int Vertical { get { return (Key == ConsoleKey.DownArrow) ? -1 : (Key == ConsoleKey.UpArrow) ? 1 : 0; } }
        public bool ESC { get { return Key == ConsoleKey.Escape; } }
        public bool Enter { get { return Key == ConsoleKey.Enter; } }
        public bool S { get { return Key == ConsoleKey.S; } }
        public bool G { get { return Key == ConsoleKey.G; } }
        public bool N { get { return Key == ConsoleKey.N; } }
        public bool R { get { return Key == ConsoleKey.R; } }
        public bool W { get { return Key == ConsoleKey.W; } }
        public bool O { get { return Key == ConsoleKey.O; } }
    }
}
