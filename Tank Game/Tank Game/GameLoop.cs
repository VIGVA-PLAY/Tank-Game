using System.Diagnostics;
using System.Windows.Threading;

namespace Tank_Game
{
    internal interface IUpdatable
    {
        void Update();
    }

    internal class GameLoop
    {
        const int TargetFPS = 60;
        DispatcherTimer _gameTimer;
        Stopwatch _stopwatch = new Stopwatch();
        public static double DeltaTime {get; private set; }

        readonly List<IUpdatable> updatables = new List<IUpdatable>();

        void InitializeTimer()
        {
            _stopwatch.Start();
            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = TimeSpan.FromMilliseconds(1000.0 / TargetFPS);
            _gameTimer.Tick += UpdateAll;
            _gameTimer.Start();
        }

        public void Start()
        {
            InitializeTimer();
        }

        void UpdateAll(object sender, EventArgs e)
        {
            DeltaTime = _stopwatch.Elapsed.TotalSeconds;
            _stopwatch.Restart();

            foreach (var updatable in updatables)
                updatable.Update();
        }

        public void RegisterUpdatable(IUpdatable updatable)
        {
            if (!updatables.Contains(updatable))
                updatables.Add(updatable);
        }

        public void UnregisterUpdatable(IUpdatable updatable)
        {
            if (updatables.Contains(updatable))
                updatables.Remove(updatable);
        }
    }
}
