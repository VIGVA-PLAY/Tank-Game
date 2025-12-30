namespace Tank_Game
{
    internal class GameObjectFactory
    {   
        readonly List<GameObject> _gameObjects = new();

        public void Destroy(GameObject go)
        {
            _gameObjects.Remove(go);
            

            if (go is IUpdatable updatable)
                GameLoop.Instance.UnregisterUpdatable(updatable);
        }

        public GameObject Instantiate<T>(Vector2 position) where T : GameObject, new()
        {
            T go = new T();
            go.Position = position;

            if (go is IUpdatable updatable)
                GameLoop.Instance.RegisterUpdatable(updatable);

            return new T();
        }
    }
}
