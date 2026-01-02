using System.Diagnostics;
using System.Windows.Threading;

namespace Tank_Game
{
    internal interface IUpdatable
    {
        void Update();
    }

    internal sealed class GameLoop
    {
        //Use reflection here in the future.

        public const int TargetFPS = 200;
        DispatcherTimer _gameTimer;
        Stopwatch _stopwatch = new Stopwatch();
        public static double DeltaTime { get; private set; }

        #region Singleton
        static readonly Lazy<GameLoop> _instance =
            new Lazy<GameLoop>(() => new GameLoop());

        GameLoop() { }

        public static GameLoop Instance => _instance.Value;
        #endregion

        readonly List<IUpdatable> _updatables = new List<IUpdatable>();
        readonly List<IUpdatable> _newUpdatables = new List<IUpdatable>();

        void InitializeTimer()
        {
            _stopwatch.Start();
            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = TimeSpan.FromMilliseconds(1000.0 / TargetFPS);
            _gameTimer.Tick += Update;
            _gameTimer.Start();
        }

        public void Run()
        {
            InitializeTimer();
        }

        void Update(object sender, EventArgs e)
        {
            DeltaTime = _stopwatch.Elapsed.TotalSeconds;
            _stopwatch.Restart();

            foreach (var updatable in _updatables)
                updatable.Update();

            if (_newUpdatables.Any())
            {
                foreach (var updatable in _newUpdatables)
                    _updatables.Add(updatable);

                _newUpdatables.Clear();
            }
        }

        public void RegisterUpdatable(IUpdatable updatable)
        {
            if (!_updatables.Contains(updatable))
                _newUpdatables.Add(updatable);
        }

        public void UnregisterUpdatable(IUpdatable updatable)
        {
            if (_updatables.Contains(updatable))
                _updatables.Remove(updatable);
        }
    }
}
