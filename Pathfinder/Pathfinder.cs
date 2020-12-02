namespace Pathfinder
{
    public class Pathfinder<T> where T : PathfinderNode<T>, IHeapItem<T>, IGrid, new()
    {        
        private ArrayList<T> _closedSet;
        private Heap<T> _openSet;
        private static ArrayList<T> _goalsToFind;
        private static ArrayList<T> _goalsFound;
        private static PathHandler<T> _paths;
        private T _current;
        private bool _blind;

        private readonly object _addPathLock = new object();
        private readonly object _addGoalLock = new object();
        private readonly object _nextGoalLock = new object();

        public System.Action<T> AddToRenderSet = delegate { };
        public PathHandler<T> Paths { get { return _paths; } }
        private int GetAbs(int i) { return i > 0 ? i : -i; }
        private int GetDistance(T a, T b)
        {
            int distX = GetAbs(a.Position.X - b.Position.X);
            int distY = GetAbs(a.Position.Y - b.Position.Y);

            if (distX > distY)
                return 14 * distY + 10 * (distX - distY);
            return 14 * distX + 10 * (distY - distX);
        }
        private T NextGoal { get { lock (_nextGoalLock) { return _goalsToFind.RemoveFirst(); } } }

        private void AddPath(Grid<T> grid, T start, T end)
        {
            lock (_addPathLock)
            {
                _paths.Add(start, end, grid);
            }
        }

        private void AddGoals(T[] goals)
        {
            lock (_addGoalLock)
            {
                if (_goalsToFind.Length > 0)
                    return;
                foreach (T node in goals)
                    _goalsToFind.Add(node);
            }
        }

        public void Reset()
        {
            _goalsToFind.Clear();
            _goalsFound.Clear();
            _paths.Clear();
        }

        private void Search(Grid<T> grid, T start, T end, bool blind)
        {
            _blind = blind;
            _openSet.Add(start);

            while (_openSet.Count > 0)
            {
                _current = _openSet.RemoveFirst();
                _closedSet.Add(_current);
                AddToRenderSet(_current);

                if (_goalsToFind.Contains(_current))
                {
                    _goalsFound.Add(_current);
                    _goalsToFind.Remove(_current);
                    AddPath(grid, start, _current);
                }

                bool foundGoal = _current == end;
                if (foundGoal || _goalsFound.Contains(end))
                {
                    if (foundGoal)
                    {
                        _goalsFound.Add(_current);
                        AddPath(grid, start, _current);
                    }
                    if (_goalsToFind.Length > 0)
                    {
                        _openSet.Clear();
                        _closedSet.Clear();
                        start = _current;
                        _openSet.Add(start);
                        end = NextGoal;
                    }
                    else
                    {
                        break;
                    }
                }

                foreach (T neighbour in _current.Neighbours)
                {
                    if(!neighbour.AccessibleFrom(_current.Position) || neighbour.Invalid || _closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMoveCostToNeighbour = _current.G + _current.Weight + GetDistance(_current, neighbour);
                    if (newMoveCostToNeighbour < neighbour.G || !_openSet.Contains(neighbour))
                    {
                        neighbour.G = newMoveCostToNeighbour;
                        neighbour.H = _blind ? 0 : GetDistance(neighbour, end);
                        neighbour.Parent = _current;

                        if (!_openSet.Contains(neighbour))
                            _openSet.Add(neighbour);
                        else
                            _openSet.HeapifyUp(neighbour);
                    }
                }
            }
        }

        public void Run(Grid<T> grid, T start, T[] goals, bool blind)
        {
            _closedSet = new ArrayList<T>(grid.Length);
            _openSet = new Heap<T>(grid.Length);
            _paths = _paths ?? new PathHandler<T>();
            _goalsFound = _goalsFound ?? new ArrayList<T>();
            _goalsToFind = _goalsToFind ?? new ArrayList<T>();
            AddGoals(goals);
            Search(grid, start, NextGoal, blind);
        }
    }
}
