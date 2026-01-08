using Tank_Game;

internal static class Function
{
    static readonly List<Timer> _activeTimers = new();

    public class Timer : IUpdate
    {
        readonly Action _callback;
        readonly double _delay;
        int _remainingRepeats;
        double _timeRemaining;
        bool _isDisposed;

        public Timer(Action callback, double delay, int repeatCount)
        {
            _callback = callback ?? throw new ArgumentNullException(nameof(callback));
            _delay = delay;
            _remainingRepeats = repeatCount;
            _timeRemaining = delay;

            GameLoop.Instance.TryRegister(this);
        }

        public void Update()
        {
            if (_isDisposed) return;

            _timeRemaining -= GameLoop.DeltaTime;

            if (_timeRemaining <= 0)
            {
                _callback?.Invoke();

                if (_remainingRepeats > 0)
                {
                    _remainingRepeats--;
                    if (_remainingRepeats == 0)
                    {
                        Dispose();
                        return;
                    }
                }
                else if (_remainingRepeats == 0)
                {
                    Dispose();
                    return;
                }

                _timeRemaining = _delay;
            }
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            GameLoop.Instance.TryUnregister(this);
            _activeTimers.Remove(this);
        }
    }

    public static Timer DelayInvoke(Action callback, double delay) =>
         StartRepeating(callback, delay, 1);

    public static Timer StartRepeating(Action callback, double delay, int repeatCount)
    {
        if (callback == null)
            throw new ArgumentNullException(nameof(callback));

        var timer = new Timer(callback, delay, repeatCount);
        _activeTimers.Add(timer);
        return timer;
    }

    public static Timer StartRepeating(Action callback, double delay) =>
         StartRepeating(callback, delay, -1);

    public static void Stop(Timer timer) => timer?.Dispose();

    public static void StopAll()
    {
        var timersCopy = _activeTimers.ToList();
        foreach (var timer in timersCopy)
            timer.Dispose();

        _activeTimers.Clear();
    }
}