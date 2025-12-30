using System.Diagnostics;
using System.Windows.Threading;

namespace Tank_Game
{
    internal interface IUpdatable
    {
        void Update();
    }

    internal interface IAwakeable
    {
        void Awake();
    }

    internal sealed class GameLoop
    {
        const int TargetFPS = 60;
        DispatcherTimer _gameTimer;
        Stopwatch _stopwatch = new Stopwatch();
        public static double DeltaTime {get; private set;}
        bool allAwakeablesAwake = false;

        #region Singleton
        static readonly Lazy<GameLoop> _instance = 
            new Lazy<GameLoop>(() => new GameLoop());

        GameLoop() { }

        public static GameLoop Instance => _instance.Value;
        #endregion

        readonly List<IAwakeable> awakeables = new List<IAwakeable>();
        readonly List<IUpdatable> updatables = new List<IUpdatable>();

        void InitializeTimer()
        {
            _stopwatch.Start();
            _gameTimer = new DispatcherTimer();
            _gameTimer.Interval = TimeSpan.FromMilliseconds(1000.0 / TargetFPS);
            _gameTimer.Tick += UpdateAll;
            _gameTimer.Start();
        }

        public void Run()
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

        public void AwakeAll()
        {
            if (allAwakeablesAwake) return;

            foreach (var awakeable in awakeables)
                awakeable.Awake();
            allAwakeablesAwake = true;
        }

        public void RegisterAwakeable(IAwakeable awakeable)
        {
            if (!awakeables.Contains(awakeable))
                awakeables.Add(awakeable);
        }

        public void UnregisterAwakeable(IAwakeable awakeable)
        {
            if (awakeables.Contains(awakeable))
                awakeables.Remove(awakeable);
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
