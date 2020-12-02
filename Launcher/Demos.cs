using System;
using System.Threading.Tasks;
using Pathfinder;

namespace Launcher
{
    public abstract class Demo
    {
        protected bool _run = true;
        protected Launcher _launcher;
        protected InputHandler _inputs;

        public virtual void Init(Launcher launcher, InputHandler inputs)
        {
            _launcher = launcher;
            _inputs = inputs;
        }

        public virtual void OnInput(object source, InputEventArgs inputArgs)
        {
            _run = !inputArgs.ESC;
        }

        public abstract void Run();
    }

    public class PathfinderDemo : Demo
    {
        private bool _editing;
        private Grid<Node> _grid;
        private Editor _editor;
        private ArrayList<Node> _renderSet;

        public bool Blind { get; set; }

        public override void OnInput(object source, InputEventArgs inputArgs)
        {
            base.OnInput(source, inputArgs);
            _editing = inputArgs.Enter || inputArgs.ESC;
        }

        public override void Init(Launcher launcher, InputHandler inputs)
        {
            base.Init(launcher, inputs);
            _grid = _grid ?? new Grid<Node>(48, 36);
            _grid.NodeRenderSize = new Point(2, 1);
            _grid.BuildNodes();
            _editor = _editor ?? new Editor();
            _editor.Init(_launcher, _inputs);
            _editor.Grid = _grid;
            _renderSet = _renderSet ?? new ArrayList<Node>(_grid.Length);
        }

        public override void Run()
        {
            _run = true;
            _editing = true;

            while (_run)
            {
                MapGenerator randomMap = new MapGenerator();
                randomMap.GenerateMap(_grid.Cols, _grid.Rows, 30);
                for (int x = 0; x < _grid.Cols; x++)
                {
                    for (int y = 0; y < _grid.Rows; y++)
                    {
                        _grid.GetNode(x, y).Type = (randomMap[x, y] == 1) ? NodeType.Obstacle : NodeType.Normal;
                    }
                }

                _editor.AddSites = true;
                _editor.Reset();
                _editor.Draw();
                _editor.Run();

                _editing = false;

                _renderSet.Clear();

                if (_editor.Finalised)
                {
                    Agent agent1 = new Agent();
                    //Agent agent2 = new Agent();
                    agent1.Blind = Blind;
                    //agent2.Blind = Blind;
                    Task task1 = new Task(() => { agent1.Run(_grid, _editor.Start, _editor.Goals); });
                    //Task task2 = new Task(() => { agent2.Run(_grid, _editor.Start, _editor.Goals); });
                    task1.Start();
                    //task2.Start();
                    task1.Wait();

                    _inputs.InputReceived += OnInput;

                    while (_editing != true)
                        _inputs.ListenForInputs();

                    _inputs.InputReceived -= OnInput;
                }
                else
                {
                    _run = false;
                }
            }
            _launcher.Reset();
        }
    }

    public class BestFirstSearch : PathfinderDemo
    {
        public override void Run()
        {
            Blind = false;
            base.Run();
        }
    }

    public class BlindSearch : PathfinderDemo
    {
        public override void Run()
        {
            Blind = true;
            base.Run();
        }
    }

    public class CartographerDemo : Demo
    {

        private bool _editing;
        private Grid<Node> _grid;
        private Editor _editor;
        private ArrayList<Node> _renderSet;

        public bool Blind { get; set; }

        public override void OnInput(object source, InputEventArgs inputArgs)
        {
            base.OnInput(source, inputArgs);
            _editing = inputArgs.Enter || inputArgs.ESC;
        }

        public override void Init(Launcher launcher, InputHandler inputs)
        {
            base.Init(launcher, inputs);
            _grid = _grid ?? new Grid<Node>(16, 12);
            _grid.NodeRenderSize = new Point(6, 3);
            _grid.BuildNodes();
            _editor = _editor ?? new Editor();
            _editor.Init(_launcher, _inputs);
            _editor.Grid = _grid;
            _renderSet = _renderSet ?? new ArrayList<Node>(_grid.Length);
        }

        public override void Run()
        {
            _run = true;
            _editing = true;

            while (_run)
            {
                _editor.AddSites = false;
                _editor.Reset();
                _editor.Draw();
                _editor.Run();

                _editing = false;

                _renderSet.Clear();

                if (_editor.Finalised)
                {
                    Cartographer<Node> cartographer = new Cartographer<Node>();
                    cartographer.GetCartogram(_grid.Nodes);

                    _inputs.InputReceived += OnInput;

                    while (_editing != true)
                        _inputs.ListenForInputs();

                    _inputs.InputReceived -= OnInput;
                }
                else
                {
                    _run = false;
                }
            }
            _launcher.Reset();
        }
    }
}

