using Pathfinder;

namespace Launcher
{
    public class Launcher
    {
        private bool _run;
        private Menu<Button> _menu;
        private InputHandler _inputs;
        private Settings _settings;        

        private Button[] Buttons
        {
            get
            {
                Button[] buttons = new Button[5];
                int spacing = 4;
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i] = new Button();
                    buttons[i].Size = new Point(37, 3);
                    buttons[i].Offset = new Point(0, (spacing * i) - 7);
                }

                buttons[0].Label = "B E S T  F I R S T  S E A R C H";
                buttons[0].Run = BestFirstSearch;

                buttons[1].Label = "B L I N D  S E A R C H";
                buttons[1].Run = BlindSearch;

                buttons[2].Label = "C A R T O G R P H E R";
                buttons[2].Run = Cartographer;

                buttons[3].Label = "N E S T E D  S E A R C H";
                buttons[3].Run = Cartographer;

                buttons[buttons.Length - 1].Label = "E X I T";
                buttons[buttons.Length - 1].Run = () => { _run = false; };

                return buttons;
            }
        }

        public delegate void ResetLauncher();
        public ResetLauncher Reset;

        public void BestFirstSearch()
        {
            _settings.SetCurrent();
            _inputs.InputReceived -= OnInput;
            Demo bestFirst = new BestFirstSearch();
            bestFirst.Init(this, _inputs);            
            bestFirst.Run();
        }

        public void BlindSearch()
        {
            _settings.SetCurrent();
            _inputs.InputReceived -= OnInput;
            Demo blind = new BlindSearch();
            blind.Init(this, _inputs);
            blind.Run();
        }

        public void Cartographer()
        {
            _settings.SetCurrent();
            _inputs.InputReceived -= OnInput;
            Demo cartographerDemo = new CartographerDemo();
            cartographerDemo.Init(this, _inputs);
            cartographerDemo.Run();
        }

        public void Run()
        {
            _run = true;

            _settings = _settings ?? new Settings();
            Reset += _settings.SetDefault;

            _inputs = _inputs ?? new InputHandler();
            Reset += () => { _inputs.InputReceived += OnInput; };

            _menu = _menu ?? new Menu<Button>();
            _menu.Create(Buttons);
            Reset += _menu.Reset;

            Reset();

            while (_run)
                _inputs.ListenForInputs();
        }

        public void OnInput(object source, InputEventArgs inputArgs)
        {
            if (inputArgs.Vertical != 0)
                _menu.SelectNext(inputArgs.Vertical);
            if (inputArgs.Enter)
                _menu.RunSelected();
        }

        static void Main()
        {
            Launcher launcher = new Launcher();
            launcher.Run();            
        }
    }
}
