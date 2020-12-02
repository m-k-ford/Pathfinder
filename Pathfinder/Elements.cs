using System;

namespace Pathfinder
{
    public abstract class Elements
    {
        protected readonly object _drawLock = new object();

        public Point Offset { get; set; }
        public Point Size { get; set; }
        public Point DefaultCursorPosition { get { return new Point((Console.WindowWidth / 2), (Console.WindowHeight / 2)); } }

        public virtual ConsoleColor Colour { get; set; }
        public abstract Point DrawPoint { get; }

        public abstract void Draw();
    }

    public class ScreenElement : Elements
    {
        public Alignment Align = new Alignment();

        public override Point DrawPoint
        {
            get
            {
                if (Align.IsLeft)
                    return new Point(Offset.X, DefaultCursorPosition.Y + Offset.Y);
                else if (Align.IsRight)
                    return new Point((Console.WindowWidth - Size.X) - Offset.X, DefaultCursorPosition.Y + Offset.Y);
                return new Point(DefaultCursorPosition.X - (Offset.X + (Size.X / 2)), DefaultCursorPosition.Y + Offset.Y);
            }
        }

        public override void Draw()
        {
            lock (_drawLock)
            {
                Console.BackgroundColor = Colour;

                for (int y = 0; y < Size.Y; y++)
                {
                    Console.SetCursorPosition(DrawPoint.X, (DrawPoint.Y + y));
                    for (int x = 0; x < Size.X; x++)
                    {
                        Console.Write(" ");
                    }
                }
            }
        }
    }

    public class TextElement : Elements
    {
        private ScreenElement _parent;

        new public Point Offset { get { return _parent.Offset; } }
        new public Point Size { get { return _parent.Size; } }
        public string Text { get; set; }

        public TextElement(ScreenElement parent)
        {
            _parent = parent;
        }

        public override Point DrawPoint
        {
            get
            {
                return new Point(DefaultCursorPosition.X - (Offset.X + (Text.Length / 2)),
                    DefaultCursorPosition.Y + (Offset.Y + (Size.Y / 2)));
            }
        }

        public override void Draw()
        {
            lock (_drawLock)
            {
                Console.ForegroundColor = Colour;
                Console.SetCursorPosition(DrawPoint.X, DrawPoint.Y);
                Console.Write(Text);
            }
        }
    }

    public class WorldElement : Elements
    {
        public int Rows { get; set; }
        public int Cols { get; set; }

        public override Point DrawPoint
        {
            get
            {
                int drawWidth = (Cols * Size.X);
                int drawHeight = (Rows * Size.Y);
                return new Point(DefaultCursorPosition.X - (drawWidth / 2) + (Offset.X * Size.X),
                    DefaultCursorPosition.Y - (drawHeight / 2) + (Offset.Y * Size.Y));
            }
        }

        public override void Draw()
        {
            lock (_drawLock)
            {
                Console.BackgroundColor = Colour;

                for (int y = 0; y < Size.Y; y++)
                {
                    Console.SetCursorPosition(DrawPoint.X, (DrawPoint.Y + y));
                    for (int x = 0; x < Size.X; x++)
                    {
                        Console.Write(" ");
                    }
                }
            }
        }

        public void Draw(ConsoleColor colour)
        {
            Console.BackgroundColor = colour;

            for (int y = 0; y < Size.Y; y++)
            {
                Console.SetCursorPosition(DrawPoint.X, (DrawPoint.Y + y));
                for (int x = 0; x < Size.X; x++)
                {
                    Console.Write(" ");
                }
            }
        }
    }

    public class Alignment
    {
        private bool _left = false;
        private bool _right = false;
        private bool _centre = true;

        public bool IsLeft { get { return _left; } }
        public bool IsRight { get { return _right; } }
        public bool IsCentre { get { return _centre; } }

        public void Left() { _left = true; _right = false; _centre = false; }
        public void Right() { _left = false; _right = true; _centre = false; }
        public void Centre() { _left = false; _right = false; _centre = true; }
    }
}
