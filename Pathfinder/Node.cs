using System.Linq;
using System.Runtime.InteropServices;

namespace Pathfinder
{
    public abstract class PathfinderNode<T> : WorldElement
    {
        private int _g;
        private int _h;

        public abstract NodeType Type { get; set; }
        public abstract T[] Neighbours { get; }
        public abstract bool Invalid { get; }
        public T Parent { get; set; }
        public int G { get { return _g; } set { _g = value; } }
        public int H { get { return _h; } set { _h = value; } }
        public int F { get { return G + H; } }
        public abstract int Weight { get; }
        public abstract Direction GetDirectionToNeighbour(Point neighbour);
        public abstract bool AccessibleFrom(Point neighbour);
        public override System.ConsoleColor Colour
        {
            get
            {
                switch (Type)
                {
                    case NodeType.Obstacle:
                        return System.ConsoleColor.Black;
                    case NodeType.Rough:
                        return System.ConsoleColor.DarkYellow;
                    case NodeType.Water:
                        return System.ConsoleColor.Blue;
                    default:
                        return System.ConsoleColor.Gray;
                }
            }
        }
    }

    public class Node : PathfinderNode<Node>, IGrid, ICartographer<Node>, IHeapItem<Node>
    {
        private readonly int[] NEIGHBOUR_INDEX = new int[] { 1, 2, 4, 7, 6, 5, 3, 0 };
        private readonly int[] CARDINAL_NEIGHBOUR_INDEX = new int[] { 1, 4, 6, 3 };
        private readonly int[] DIAGONAL_NEIGHBOUR_INDEX = new int[] { 2, 7, 5, 0 };


        private int _obstacleWeight = 100;
        private int _roughtWeight = 20;
        private int _waterWeight = 30;
        private int _defaultWeight = 0;
        private NodeType _type;

        private Grid<Node> _parentGrid;
        private Node[] _neighbours;
        private Point _position;
        private int[] _obstacle;

        public override NodeType Type { get { return _type; } set { AssignObstacle(value); _type = value; } }
        public int HeapIndex { get; set; }
        public Point Position { get { return _position; } }
        public int X { get { return _position.X; } }
        public int Y { get { return _position.Y; } }
        public override bool Invalid { get { return Type == NodeType.Obstacle || _parentGrid.OutOfBounds(Position); } }
        public override Node[] Neighbours { get { if (_neighbours == null) AssignNeighbours(); return _neighbours; } }

        public override int Weight
        {
            get
            {
                switch (Type)
                {
                    case NodeType.Obstacle:
                        return _obstacleWeight;
                    case NodeType.Rough:
                        return _roughtWeight;
                    case NodeType.Water:
                        return _waterWeight;
                    default:
                        return _defaultWeight;
                }
            }
        }

        public int CompareTo(Node nodeToCompare)
        {
            int compare = F.CompareTo(nodeToCompare.F);
            if (compare == 0)
            {
                compare = H.CompareTo(nodeToCompare.H);
            }
            return -compare;
        }

        private void AssignNeighbours()
        {
            _neighbours = new Node[8];
            int index = 0;
            Point coordinates = Point.Nil;

            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (y == 0 && x == 0)
                        continue;

                    coordinates.X = x;
                    coordinates.Y = y;
                    _neighbours[index] = _parentGrid.GetNode(Position + coordinates);
                    index++;
                }
            }
        }

        private void AssignObstacle(NodeType type)
        {
            _obstacle = new int[8];
            for (int i = 0; i < _obstacle.Length; i++)
            {
                _obstacle[i] = (int)type;
            }
        }

        public override Direction GetDirectionToNeighbour(Point neighbour)
        {
            for (int i = 0; i < Neighbours.Length; i++)
            {
                if (_neighbours[i].Position == neighbour)
                    return (Direction)i;
            }
            return Direction.NorthWest;
        }

        public override bool AccessibleFrom(Point neighbour)
        {
            int index = 0;
            for (int i = 0; i < Neighbours.Length; i++)
            {
                if (_neighbours[i].Position == neighbour)
                {
                    index = i;
                    break;
                }
            }

            if (DIAGONAL_NEIGHBOUR_INDEX.Contains(index))
            {                
                int next = (index < 7) ? (index + 1) : 0;
                int previous = (index > 0) ? (index - 1) : 7;
                if (_neighbours[NEIGHBOUR_INDEX[previous]].Invalid || _neighbours[NEIGHBOUR_INDEX[next]].Invalid)
                {
                    return false;
                }
                return true;
            }
            return _obstacle[index] != (int)NodeType.Obstacle;
        }

        public Node GetNeighbour(Direction direction)
        {
            if (_neighbours == null)
                AssignNeighbours();
            return _neighbours[(int)direction];
        }

        public void Init(object grid, Point size, Point position)
        {
            _parentGrid = (Grid<Node>)grid;
            _position = position;
            Offset = position;
            Size = size;
            Rows = _parentGrid.Rows;
            Cols = _parentGrid.Cols;
            Type = NodeType.Normal;
        }
    }

    public enum NodeType
    {
        Obstacle = -1,
        Normal = 1,
        Rough = 2,
        Water = 3
    }

    public enum Direction
    {
        North = 1,
        NorthEast = 2,
        East = 4,
        SouthEast = 7,
        South = 6,
        SouthWest = 5,
        West = 3,
        NorthWest = 0
    }
}
