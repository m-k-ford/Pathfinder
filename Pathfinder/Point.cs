using System;

namespace Pathfinder
{
    public struct Point
    {    
        public int X { get; set; }
        public int Y { get; set; }

        public static Point Nil { get { return new Point(0, 0); } }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        #region Overrides
        public static Point operator +(Point a, Point b) => new Point(a.X += b.X, a.Y += b.Y);
        public static Point operator -(Point a, Point b) => new Point(a.X -= b.X, a.Y -= b.Y);
        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Point p = (Point)obj;
                return (X == p.X) && (Y == p.Y);
            }
        }

        public static bool operator ==(Point a, Point b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            if (ReferenceEquals(a, null))
            {
                return false;
            }
            if (ReferenceEquals(b, null))
            {
                return false;
            }

            return a.Equals(b);
        }
        public static bool operator !=(Point a, Point b)
        {
            return !(a == b);
        }
        public override int GetHashCode()
        {
            return (X << 2) ^ Y;
        }
        public override string ToString() => $"{X}, {Y}";
#endregion
    }
}
