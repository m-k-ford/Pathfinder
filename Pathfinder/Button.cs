using System;

namespace Pathfinder
{
    public sealed class Button : ScreenElement, IMenuItem
    {
        private bool _isSelected;
        private TextElement _label;

        public override ConsoleColor Colour { get { return _isSelected ? ConsoleColor.DarkGray : ConsoleColor.White; } }
        public string Label { get { return _label.Text; } set { _label.Text = value; } }
        public Action Run { get; set; }

        public Button()
        {
            _label = new TextElement(this);
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                Draw();
                _label.Colour = _isSelected ? ConsoleColor.Yellow : ConsoleColor.DarkGray;
                _label.Draw();
            }
        }
    }
}
