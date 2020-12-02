using System;
using System.Collections.Generic;
using System.Linq;
using Pathfinder;

namespace Launcher
{
    public class Editor : Demo
    {
        private SiteManager _sites = new SiteManager();
        private Grid<Node> _grid;
        private Node _current;
        private EditorColours _colours;
        private UserInterface _ui;
        public bool AddSites { get; set; }
        public bool Finalised { get { return (AddSites) ? _sites.Count > 1 : true; } }
        public Node Start { get { return _sites.Start ?? new Node(); } }
        public Node[] Goals { get { return _sites.Goals; } }
        public Node Goal { get { return _sites.Goal ?? new Node(); } }
        public Grid<Node> Grid
        {
            get
            {
                return _grid;
            }
            set
            {
                if (_grid != null)
                {
                    foreach (Node node in _grid.Nodes)
                        Draw -= node.Draw;
                }
                foreach (Node node in value.Nodes)
                    Draw += node.Draw;
                _grid = value;
            }
        }

        public delegate void DrawGrid();
        public DrawGrid Draw;

        private void DrawSelected()
        {
            if (_sites.NodeIsFlagged(_current))
            {
                ConsoleColor colour = (_sites.GetFlag(_current) == SiteFlag.Start) ? _colours.Start : _colours.Goal;
                _current.Draw(colour);
            }
            else
            {
                _current.Draw();
            }
        }

        private void SelectNew(InputEventArgs inputArgs)
        {
            Point position = _current.Position;
            position.X += inputArgs.Horizontal;
            position.Y -= inputArgs.Vertical;

            if (_grid.OutOfBounds(position))
                return;

            DrawSelected();

            _current = _grid.GetNode(position);
            _current.Draw(_colours.Selected);
        }

        private void ModifySelected(InputEventArgs inputArgs)
        {
            if (inputArgs.N)
                _current.Type = NodeType.Normal;
            if (inputArgs.O)
                _current.Type = NodeType.Obstacle;
            if (inputArgs.W)
                _current.Type = NodeType.Water;
            if (inputArgs.R)
                _current.Type = NodeType.Rough;
            if (AddSites)
            {
                if (inputArgs.S)
                    _sites.AddSelected(SiteFlag.Start, _current);
                if (inputArgs.G)
                    _sites.AddSelected(SiteFlag.Goal, _current);
            }
        }

        public override void OnInput(object source, InputEventArgs inputArgs)
        {
            base.OnInput(source, inputArgs);
            SelectNew(inputArgs);
            ModifySelected(inputArgs);
            if (inputArgs.Enter)
            {
                if (!Finalised)
                    return;
                _run = false;
            }
        }

        public override void Run()
        {
            _run = true;

            _ui = _ui ?? new UserInterface();
            _ui.Draw();
            //UIElement instructions = new UIElement();
            //instructions.Label.Text = "Instructions";
            //instructions.Draw();

            _colours = new EditorColours();
            _current = _grid.GetNode(0, 0);
            _current.Draw(_colours.Selected);

            _inputs.InputReceived += OnInput;

            while (_run)
                _inputs.ListenForInputs();

            _inputs.InputReceived -= OnInput;
            DrawSelected();
        }

        public void Reset()
        {
            _sites.Clear();
        }

        private struct EditorColours
        {
            public ConsoleColor Default { get { return ConsoleColor.Gray; } }
            public ConsoleColor Selected { get { return ConsoleColor.White; } }
            public ConsoleColor Start { get { return ConsoleColor.Green; } }
            public ConsoleColor Goal { get { return ConsoleColor.Red; } }
        }
    }

    public enum SiteFlag
    {
        Start,
        Goal
    }

    public class SiteManager
    {
        private Dictionary<Node, SiteFlag> _siteData = new Dictionary<Node, SiteFlag>();

        public int Count { get { return _siteData.Count; } }
        public int GoalCount { get { return _siteData.Count - 1; } }
        public Node Start { get; set; }
        public Node Goal { get; set; }
        public Node[] Goals { get { return (from n in _siteData where n.Value == SiteFlag.Goal select n.Key).ToArray(); } }

        public Action Clear => delegate { _siteData.Clear(); };

        public bool NodeIsFlagged(Node current)
        {
            return _siteData.ContainsKey(current);
        }

        public SiteFlag GetFlag(Node current)
        {
            return _siteData[current];
        }

        public void AddSelected(SiteFlag flag, Node current)
        {
            if (flag == SiteFlag.Start)
            {
                if (Start != null)
                {
                    _siteData.Remove(Start);
                    Start.Draw();
                }
                Start = current;
            }
            else if (flag == SiteFlag.Goal && _siteData.ContainsKey(current))
            {
                _siteData.Remove(current);
                current.Draw();
                return;
            }
            _siteData.Add(current, flag);
        }
    }
}
