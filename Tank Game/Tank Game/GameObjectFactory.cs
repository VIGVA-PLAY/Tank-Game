namespace Tank_Game
{
    internal class GameObjectFactory
    {   
        readonly List<GameObject> _gameObjects = new();
        //This class needs to be a singleton

        public void Destroy(GameObject go)
        {
            _gameObjects.Remove(go);
            go.Dispose();

            if (go is IUpdatable updatable)
                GameLoop.Instance.UnregisterUpdatable(updatable);
        }

        public T Instantiate<T>(Vector2 position) where T : GameObject, new()
        {
            T go = new();
            go.Position = position;
            _gameObjects.Add(go);

            if (go is IUpdatable updatable)
                GameLoop.Instance.RegisterUpdatable(updatable);

            go.Awake();
            return go;
        }

        public int GetGameObjectCount() => _gameObjects.Count;
    }
}
