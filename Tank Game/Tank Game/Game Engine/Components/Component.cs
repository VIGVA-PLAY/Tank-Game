namespace Tank_Game
{
    internal class Component : IDisposable
    {
        public GameObject gameObject;
        bool _awoken;

        public void Awake()
        {
            if (_awoken) return;
            _awoken = true;

            OnAwake();
        }

        protected virtual void OnAwake() { }
        protected virtual void OnDispose() { }

        public void Dispose()
        {
            OnDispose();
        }
    }
}
