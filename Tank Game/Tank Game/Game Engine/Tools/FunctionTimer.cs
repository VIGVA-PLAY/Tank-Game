using System.Windows.Media.Animation;
using Tank_Game;

internal static class FunctionTimer
{
    static readonly List<Timer> _activeTimers = new();

    public class Timer
    {
        readonly Action callback;
        readonly double delay;
        int repeatCount;
        double timeRemaining;

        public Timer(Action callback, double delay, int repeatCount)
        {
            this.callback = callback;
            this.delay = delay;
            this.repeatCount = repeatCount;
            timeRemaining = delay;

            GameLoop.Instance.OnUpdate += Update;
        }

        void Update()
        {
            timeRemaining -= GameLoop.DeltaTime;

            if (timeRemaining <= 0) 
            {
                if (repeatCount < 0)
                {
                    callback?.Invoke();
                    return;
                }

                repeatCount--;

                if (repeatCount == 0)
                {
                    Dispose();
                    return;
                }
              
                timeRemaining = delay;
            }
        }

        public void Dispose()
        {
            GameLoop.Instance.OnUpdate -= Update;
            _activeTimers.Remove(this);
        }
    }

    public static void Start(Action onComplete, double delay) =>
        StartRepeating(onComplete, delay, 1);

    public static void StartRepeating(Action onComplete, double delay, int repeatCount)
    {
        if (onComplete != null) return;
        _activeTimers.Add(new Timer(onComplete, delay, repeatCount));
    }

    public static void StartRepeating(Action onComplete, double delay) =>
        StartRepeating(onComplete,  delay, -1);

    public static void Stop(Timer timer) => timer?.Dispose();

    public static void StopAll()
    {
        foreach(Timer timer in _activeTimers) timer.Dispose();
    } 
}