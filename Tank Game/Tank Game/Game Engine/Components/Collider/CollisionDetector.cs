namespace Tank_Game
{
    internal sealed class CollisionDetector : IFixedUpdate
    {
        readonly CollectionRegistry<Collider> _collidersRegistry = new();
        readonly Dictionary<(Collider, Collider), bool> _collisionStates = new();

        //Singleton
        static readonly Lazy<CollisionDetector> _instance = new(() => new CollisionDetector());
        public static CollisionDetector Instance => _instance.Value;

        CollisionDetector() => GameLoop.Instance.TryRegister(this);

        public void FixedUpdate()
        {
            _collidersRegistry.ProcessPendingChanges();
            Detect();
        }

        void Detect()
        {
            var items = _collidersRegistry.Items;

            for (int i = 0; i < items.Count; i++)
            {
                for (int j = i + 1; j < items.Count; j++)
                {
                    var collider1 = items[i];
                    var collider2 = items[j];

                    if (collider1.gameObject == collider2.gameObject)
                        continue;

                    bool isColliding = collider1.Intersects(collider2);

                    var key = (collider1, collider2);
                    bool wasColliding = _collisionStates.TryGetValue(key, out bool prev) && prev;

                    if (isColliding && !wasColliding)
                    {
                        // Collision Enter
                        _collisionStates[key] = true;
                        collider1.gameObject.OnCollisionEnter(collider2);
                        collider2.gameObject.OnCollisionEnter(collider1);
                    }
                    else if (isColliding && wasColliding)
                    {
                        // Collision Stay
                        collider1.gameObject.OnCollisionStay(collider2);
                        collider2.gameObject.OnCollisionStay(collider1);
                    }
                    else if (!isColliding && wasColliding)
                    {
                        // Collision Exit
                        _collisionStates[key] = false;
                        collider1.gameObject.OnCollisionExit(collider2);
                        collider2.gameObject.OnCollisionExit(collider1);
                    }
                }
            }
        }

        public void Register(Collider collider) => _collidersRegistry.Register(collider);

        public void Unregister(Collider collider)
        {
            _collidersRegistry.Unregister(collider);

            var keysToRemove = _collisionStates.Keys
                .Where(k => k.Item1 == collider || k.Item2 == collider)
                .ToList();

            foreach (var key in keysToRemove)
                _collisionStates.Remove(key);
        }

        public int GetColliderCount() => _collidersRegistry.Count;

        public void Clear()
        {
            _collidersRegistry.Clear();
            _collisionStates.Clear();
        }
    }
}

