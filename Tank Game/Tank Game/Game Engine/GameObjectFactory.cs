namespace Tank_Game
{
    internal sealed class GameObjectFactory
    {
        readonly List<GameObject> _gameObjects = new();

        #region Singleton
        static readonly Lazy<GameObjectFactory> _instance =
            new Lazy<GameObjectFactory>(() => new GameObjectFactory());

        public static GameObjectFactory Instance => _instance.Value;

        GameObjectFactory() { }
        #endregion

        public void Dispose(GameObject go)
        {
            _gameObjects.Remove(go);

            if (go is IUpdatable updatable)
                GameLoop.Instance.UnregisterUpdatable(updatable);
        }

        public T Instantiate<T>(Vector2 position = default, double rotation = 0)
            where T : GameObject, new()
        {
            T go = new();
            go.Position = position;
            go.Rotation = rotation;
            _gameObjects.Add(go);

            //Awake must be called before Update method. 
            //Better solution needed.
            go.Awake();

            if (go is IUpdatable updatable)
                GameLoop.Instance.RegisterUpdatable(updatable);

            return go;
        }

        public T Instantiate<T>(double x = 0, double y = 0, double rotation = 0)
            where T : GameObject, new() => Instantiate<T>(new Vector2(x, y), rotation);

        public T Instantiate<T>() where T : GameObject, new() =>
            Instantiate<T>(default, 0);

        public int GetGameObjectCount() => _gameObjects.Count;
    }
}
