using System.Diagnostics;
using System.Windows.Threading;

namespace Tank_Game
{
    internal interface IUpdate : IUpdatable
    {
        void Update();
    }

    internal interface IFixedUpdate : IUpdatable
    {
        void FixedUpdate();
    }

    internal interface ILateUpdate : IUpdatable
    {
        void LateUpdate();
    }

    internal interface IUpdatable { }

    internal sealed class GameLoop
    {
        public const int TargetFPS = 120;
        public const double FixedTimeStep = 1.0 / 50.0;
        const int MaxFixedUpdatesPerFrame = 5;

        DispatcherTimer _gameTimer;
        readonly Stopwatch _stopwatch = new();
        bool _isRunning = false;
        bool _isPaused = false;

        double _fixedUpdateAccumulator = 0.0;
        int _frameCount = 0;
        double _fpsUpdateTimer = 0.0;
        double _currentFPS = 0.0;

        public static double DeltaTime { get; private set; }
        public static double UnscaledDeltaTime { get; private set; }
        public static double Time { get; private set; }
        public static double FixedDeltaTime => FixedTimeStep;
        public static double TimeScale { get; set; } = 1.0;
        public double CurrentFPS => _currentFPS;

        //Singleton
        static readonly Lazy<GameLoop> _instance = new(() => new GameLoop());
        public static GameLoop Instance => _instance.Value;
        GameLoop() { }

        readonly UpdatableRegistry<IUpdate> _updateRegitry = new();
        readonly UpdatableRegistry<IFixedUpdate> _fixedUpdateRegistry = new();
        readonly UpdatableRegistry<ILateUpdate> _lateUpdateRegistry = new();

        public void Run()
        {
            if (_isRunning)
            {
                Debug.WriteLine("GameLoop is already running!");
                return;
            }

            _isRunning = true;
            _isPaused = false;
            Time = 0.0;
            InitializeTimer();

            Debug.WriteLine($"GameLoop started - Target FPS: {TargetFPS}, Fixed Step: {FixedTimeStep:F4}s ({1.0 / FixedTimeStep:F0} FPS)");
        }

        public void Stop()
        {
            if (!_isRunning) return;

            _gameTimer?.Stop();
            _stopwatch?.Stop();
            _isRunning = false;
            _isPaused = false;

            Debug.WriteLine("GameLoop stopped");
        }

        public void Pause()
        {
            if (!_isRunning || _isPaused) return;

            _gameTimer?.Stop();
            _stopwatch?.Stop();
            _isPaused = true;

            Debug.WriteLine("GameLoop paused");
        }

        public void Resume()
        {
            if (!_isRunning || !_isPaused) return;

            _gameTimer?.Start();
            _stopwatch?.Start();
            _isPaused = false;

            Debug.WriteLine("GameLoop resumed");
        }

        void InitializeTimer()
        {
            _stopwatch.Restart();

            _gameTimer = new DispatcherTimer(DispatcherPriority.Render);
            _gameTimer.Interval = TimeSpan.FromMilliseconds(1000.0 / TargetFPS);
            _gameTimer.Tick += GameLoopTick;
            _gameTimer.Start();
        }

        void GameLoopTick(object sender, EventArgs e)
        {
            UnscaledDeltaTime = _stopwatch.Elapsed.TotalSeconds;
            _stopwatch.Restart();

            if (UnscaledDeltaTime > 0.1)
            {
                UnscaledDeltaTime = 0.1;
                Debug.WriteLine("Warning: Frame took too long, clamping DeltaTime");
            }

            DeltaTime = UnscaledDeltaTime * TimeScale;
            Time += DeltaTime;

            UpdateFPSCounter();
            ProcessPendingChanges();

            _fixedUpdateAccumulator += UnscaledDeltaTime;

            int fixedUpdateCount = 0;
            while (_fixedUpdateAccumulator >= FixedTimeStep && fixedUpdateCount < MaxFixedUpdatesPerFrame)
            {
                ProcessFixedUpdate();
                _fixedUpdateAccumulator -= FixedTimeStep;
                fixedUpdateCount++;
            }

            if (fixedUpdateCount >= MaxFixedUpdatesPerFrame)
            {
                _fixedUpdateAccumulator = 0;
                Debug.WriteLine($"Warning: Fixed update spiral prevented ({fixedUpdateCount} updates)");
            }

            ProcessUpdate();
            ProcessLateUpdate();
        }

        void ProcessUpdate() => _updateRegitry.ProcessAll(u => u.Update());
        void ProcessFixedUpdate() => _fixedUpdateRegistry.ProcessAll(fu => fu.FixedUpdate());
        void ProcessLateUpdate() => _lateUpdateRegistry.ProcessAll(lu => lu.LateUpdate());

        void ProcessPendingChanges()
        {
            _updateRegitry.ProcessPendingChanges();
            _fixedUpdateRegistry.ProcessPendingChanges();
            _lateUpdateRegistry.ProcessPendingChanges();
        }

        public void TryRegister<T>(T obj) where T : class
        {
            if (obj is null) return;
            if (obj is IUpdate updatable) _updateRegitry.Register(updatable);
            if (obj is IFixedUpdate fixedUpdatable) _fixedUpdateRegistry.Register(fixedUpdatable);
            if (obj is ILateUpdate lateUpdatable) _lateUpdateRegistry.Register(lateUpdatable);
        }

        public void TryUnregister<T>(T obj) where T : class
        {
            if (obj is null) return;
            if (obj is IUpdate updatable) _updateRegitry.Unregister(updatable);
            if (obj is IFixedUpdate fixedUpdatable) _fixedUpdateRegistry.Unregister(fixedUpdatable);
            if (obj is ILateUpdate lateUpdatable) _lateUpdateRegistry.Unregister(lateUpdatable);
        }

        void UpdateFPSCounter()
        {
            _frameCount++;
            _fpsUpdateTimer += UnscaledDeltaTime;

            if (_fpsUpdateTimer >= 1.0)
            {
                _currentFPS = _frameCount / _fpsUpdateTimer;
                _frameCount = 0;
                _fpsUpdateTimer = 0.0;
            }
        }

        public int GetUpdateCount() => _updateRegitry.Count;
        public int GetFixedUpdateCount() => _fixedUpdateRegistry.Count;
        public int GetLateUpdateCount() => _lateUpdateRegistry.Count;
        public int GetTotalCount() => GetUpdateCount() + GetFixedUpdateCount() + GetLateUpdateCount();

        public void ClearAll()
        {
            _updateRegitry.Clear();
            _fixedUpdateRegistry.Clear();
            _lateUpdateRegistry.Clear();
        }

        public void PrintDebugInfo()
        {
            Debug.WriteLine("==================== GameLoop Debug Info ====================");
            Debug.WriteLine($"Running: {_isRunning} | Paused: {_isPaused}");
            Debug.WriteLine($"FPS: {_currentFPS:F1} | DeltaTime: {DeltaTime:F4}s | Time: {Time:F2}s");
            Debug.WriteLine($"TimeScale: {TimeScale:F2}");
            Debug.WriteLine($"Update: {_updateRegitry.Count} | FixedUpdate: {_fixedUpdateRegistry.Count} | LateUpdate: {_lateUpdateRegistry.Count}");
            Debug.WriteLine($"Fixed Accumulator: {_fixedUpdateAccumulator:F4}s");
            Debug.WriteLine("=============================================================");
        }
    }
}