using System;
namespace Pathfinder
{
    public class Agent
    {
        private bool _blind;
        private Pathfinder<Node> _pathfinder;
        private static ArrayList<Node> _renderSet;

        private readonly object _renderSetLock = new object();

        public bool Blind { get { return _blind; } set { _blind = value; } }
        //public Action Reset;
        public Agent()
        {
            _pathfinder = new Pathfinder<Node>();
            _renderSet = _renderSet ?? new ArrayList<Node>();
            _pathfinder.AddToRenderSet += AddToRenderSet;
            //Reset = () => {  };
        }

        private void AddToRenderSet(Node nodeToAdd)
        {
            lock (_renderSetLock)
            {
                _renderSet.Add(nodeToAdd);
            }
        }

        public void Run(Grid<Node> grid, Node start, Node[] goals)
        {
            _pathfinder.Run(grid, start, goals, _blind);

            for (int i = 0; i < _renderSet.Length; i++)
            {
                _renderSet[i].Draw(ConsoleColor.White);
                System.Threading.Thread.Sleep(16);
                _renderSet[i].Draw(ConsoleColor.Magenta);
            }

            for (int i = 0; i < _pathfinder.Paths.Count; i++)
            {
                _pathfinder.Paths[i].Draw(ConsoleColor.Yellow);
            }

            start.Draw(ConsoleColor.Green);
            for (int i = 0; i < goals.Length; i++)
                goals[i].Draw(ConsoleColor.Red);

            _pathfinder.Reset();
            _renderSet.Clear();
        }
    }
}
