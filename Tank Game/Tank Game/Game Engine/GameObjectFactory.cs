namespace Tank_Game
{
    internal sealed class GameObjectFactory
    {
        readonly List<GameObject> _gameObjects = new();

        //Singleton
        static readonly Lazy<GameObjectFactory> _instance = new(() => new GameObjectFactory());
        public static GameObjectFactory Instance => _instance.Value;
        GameObjectFactory() { }

        public void Delete(GameObject go)
        {
            if (go == null) return;
            if (!_gameObjects.Remove(go)) return;

            UnregisterFromGameLoop(go);
        }

        void RegisterToGameLoop(GameObject go)
        {
            if (go is IUpdate updatable)
                GameLoop.Instance.TryRegister(updatable);

            if (go is IFixedUpdate fixedUpdatable)
                GameLoop.Instance.TryRegister(fixedUpdatable);

            if (go is ILateUpdate lateUpdatable)
                GameLoop.Instance.TryRegister(lateUpdatable);
        }

        void UnregisterFromGameLoop(GameObject go)
        {
            if (go is IUpdate updatable)
                GameLoop.Instance.Unregister(updatable);

            if (go is IFixedUpdate fixedUpdatable)
                GameLoop.Instance.Unregister(fixedUpdatable);

            if (go is ILateUpdate lateUpdatable)
                GameLoop.Instance.Unregister(lateUpdatable);
        }

        public T Instantiate<T>(Vector2 position = default, double rotation = 0)
            where T : GameObject, new()
        {
            T go = new();
            go.Position = position;
            go.Rotation = rotation;

            _gameObjects.Add(go);

            go.Awake();
            RegisterToGameLoop(go);

            return go;
        }

        public T Instantiate<T>(double x, double y, double rotation = 0) where T : GameObject, new() =>
            Instantiate<T>(new Vector2(x, y), rotation);

        public T Instantiate<T>() where T : GameObject, new() =>
            Instantiate<T>(Vector2.Zero, 0);

        public int GetGameObjectsCount() => _gameObjects.Count;

        public List<GameObject> GetAllGameObjects() => _gameObjects.ToList();

        public T FindGameObject<T>() where T : GameObject =>
            _gameObjects.OfType<T>().FirstOrDefault();

        public List<T> FindGameObjects<T>() where T : GameObject =>
            _gameObjects.OfType<T>().ToList();

        public GameObject FindGameObject(Predicate<GameObject> predicate) =>
            _gameObjects.FirstOrDefault(go => predicate?.Invoke(go) ?? false);

        public void DestroyAll()
        {
            var gameObjectsCopy = _gameObjects.ToList();
            foreach (var go in gameObjectsCopy)
                go.Destroy();
        }

        public void DestroyAllOfType<T>() where T : GameObject
        {
            var objectsToDestroy = FindGameObjects<T>();
            foreach (var go in objectsToDestroy)
                go.Destroy();
        }
    }
}

