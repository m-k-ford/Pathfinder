using System.Linq;

namespace Pathfinder
{
    public sealed class Grid<T> where T : IGrid, new()
    {
        private T[,] _nodes;
        private Point _nodeRenderSize;

        public int Length { get { return _nodes.Length; } }
        public int Cols { get { return _nodes.GetLength(0); } }
        public int Rows { get { return _nodes.GetLength(1); } }
        public T[,] Nodes { get { return _nodes; } }
        public Point NodeRenderSize { get { return _nodeRenderSize; } set { _nodeRenderSize = value; } }

        public Grid(int x, int y)
        {
            _nodes = new T[x, y];
        }

        public T[] ToArray()
        {
            return _nodes.Cast<T>().ToArray();
        }

        public bool OutOfBounds(Point position)
        {
            if (position.X < 0 || position.X > (Cols - 1) || position.Y < 0 || position.Y > (Rows - 1))
                return true;
            return false;
        }

        public T GetDummyNode(Point position)
        {
            T temp = new T();
            temp.Init(this, _nodeRenderSize, position);
            return temp;
        }

        public T GetDummyNode(int x, int y)
        {
            T temp = new T();
            temp.Init(this, _nodeRenderSize, new Point(x, y));
            return temp;
        }

        public T GetNode(Point position)
        {
            if (!OutOfBounds(position))
                return _nodes[position.X, position.Y];
            return GetDummyNode(position);
        }

        public T GetNode(int x, int y)
        {
            if (!OutOfBounds(new Point(x, y)))
                return _nodes[x, y];
            return GetDummyNode(x, y);
        }

        public T GetNode(int i)
        {
            if (i > 0 && i < (_nodes.Length - 1))
                return ToArray()[i];
            return GetDummyNode(i, i);
        }

        public T[] GetRow(int y)
        {
            if (!OutOfBounds(new Point(0, y)))
            {
                return (from node in ToArray()
                        where node.Position.Y == y
                        select node).ToArray();
            }
            return new T[] { GetDummyNode(new Point(0, y)) };
        }

        public T[] GetCol(int x)
        {
            if (!OutOfBounds(new Point(x, 0)))
            {
                return (from node in ToArray()
                        where node.Position.X == x
                        select node).ToArray();
            }
            return new T[] { GetDummyNode(new Point(x, 0)) };
        }

        public void BuildNodes()
        {
            for (int x = 0; x < _nodes.GetLength(0); x++)
            {
                for (int y = 0; y < _nodes.GetLength(1); y++)
                {
                    _nodes[x, y] = new T();
                    _nodes[x, y].Init(this, _nodeRenderSize, new Point(x, y));
                }
            }            
        }
    }

    public interface IGrid
    {
        void Init(object grid, Point size, Point position);
        Point Position { get; }
    }
}