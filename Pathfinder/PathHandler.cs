using System;

namespace Pathfinder
{
    public class PathHandler<T> where T : PathfinderNode<T>, IGrid, new()
    {
        private ArrayList<Path> _paths = new ArrayList<Path>();

        public Path this[int i] { get { return _paths[i]; } }
        public int Count { get { return _paths.Length; } }

        public System.Action Clear => delegate { _paths.Clear(); };

        private Path MakePath(T startNode, T endNode, Grid<T> grid)
        {
            Path temp = new Path();
            System.Collections.Generic.List<Point> path = new System.Collections.Generic.List<Point>();
            T currentNode = endNode;
            while (currentNode.Position != startNode.Position)
            {
                path.Add(currentNode.Position);
                temp.Draw += currentNode.Draw;
                currentNode = currentNode.Parent;
            }
            path.Reverse();
            temp.SetPath(path.ToArray());
            return temp;
        }

        public void Add(T startNode, T endNode, Grid<T> grid)
        {
            _paths.Add(MakePath(startNode, endNode, grid));
        }

        public void Remove(Path pathToRemove)
        {
            for (int i = 0; i < _paths.Length; i++)
            {
                if (pathToRemove != _paths[i])
                    continue;

                _paths.Remove(pathToRemove);
                break;
            }
        }
    }

    public class Path
    {
        private Point[] _path;
        public Point this[int i] { get { return _path[i]; } }
        public int Length { get { return _path.Length; } }
        public Point Start { get { return _path[0]; } }
        public Point End { get { return _path[Length - 1]; } }

        public delegate void DrawPath(ConsoleColor color);
        public DrawPath Draw;

        public void SetPath(Point[] path)
        {
            _path = path;
        }

        #region Overrides
        public override bool Equals(object obj)
        {
            if ((obj == null) || !GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Path p = (Path)obj;
                return (Start == p.Start) && (End == p.End);
            }
        }

        public static bool operator ==(Path a, Path b)
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

        public static bool operator !=(Path a, Path b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return _path[0].GetHashCode() + _path[Length - 1].GetHashCode();
        }
        #endregion
    }
}
