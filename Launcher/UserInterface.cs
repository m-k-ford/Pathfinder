using System;
using Pathfinder;

namespace Launcher
{
    public class UserInterface : ScreenElement
    {
        public UserInterface()
        {
            Align.Right();
            int height = 36;
            Size = new Point(20, height);
            Offset = new Point(4, -(height / 2));
            Colour = ConsoleColor.DarkGray;            
        }
    }

    public class UIElement : ScreenElement
    {
        public TextElement Label { get; private set; }

        public UIElement()
        {
            Label = new TextElement(this);
        }
    }
}
