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

            GameLoop.Instance.TryUnregister(go);
        }

        public T Instantiate<T>(Vector2 position = default, double rotation = 0)
            where T : GameObject, new()
        {
            T go = new() { Position = position, Rotation = rotation };

            _gameObjects.Add(go);

            go.Awake();
            GameLoop.Instance.TryRegister(go);

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

