namespace Tank_Game
{
    using System;
    using System.Windows.Controls;

    internal abstract class GameObject
    {
        protected readonly Canvas gameCanvas = MainWindow.Instance.GameCanvas;

        bool _awoken;

        readonly List<GameObject> _children = new();
        readonly List<Component> _components = new();

        Vector2 _position;
        double _rotation;
        Vector2 _localPosition;
        double _localRotation;

        public event Action OnTransform;
        public GameObject Parent { get; private set; }

        public Vector2 Position
        {
            get => _position;
            set
            {
                if (_position == value) return;
                _position = value;
                OnTransform?.Invoke();
                UpdateChildrenPositions();
            }
        }

        public double Rotation
        {
            get => _rotation;
            set
            {
                if (Math.Abs(_rotation - value) < 0.001) return;
                _rotation = value;
                OnTransform?.Invoke();
                UpdateChildrenRotations();
            }
        }

        public Vector2 LocalPosition
        {
            get => _localPosition;
            set
            {
                _localPosition = value;
                UpdateWorldPosition();
            }
        }

        public double LocalRotation
        {
            get => _localRotation;
            set
            {
                _localRotation = value;
                UpdateWorldRotation();
            }
        }

        void UpdateWorldPosition()
        {
            if (Parent is not null)
            {
                double radians = Parent.Rotation;
                double cos = Math.Cos(radians);
                double sin = Math.Sin(radians);

                Vector2 rotatedLocal = new(
                    _localPosition.x * cos - _localPosition.y * sin,
                    _localPosition.x * sin + _localPosition.y * cos
                );

                Position = Parent.Position + rotatedLocal;
            }
            else
                Position = _localPosition;
        }

        void UpdateWorldRotation()
        {
            if (Parent is not null)
                Rotation = Parent.Rotation + _localRotation;
            else
                Rotation = _localRotation;
        }

        void UpdateChildrenPositions()
        {
            if (_children.Count == 0) return;

            foreach (var child in _children)
                child.UpdateWorldPosition();
        }

        void UpdateChildrenRotations()
        {
            if (_children.Count == 0) return;

            foreach (var child in _children)
            {
                child.UpdateWorldPosition();
                child.UpdateWorldRotation();
            }
        }

        public void AddChild(GameObject child)
        {
            if (child is null)
                throw new ArgumentNullException(nameof(child));

            if (child == this)
                throw new InvalidOperationException("GameObject cannot be its own child.");

            if (IsDescendantOf(child))
                throw new InvalidOperationException("Cannot create cyclic hierarchy.");

            child.DetachFromParent();

            child.Parent = this;
            _children.Add(child);

            child.UpdateWorldPosition();
            child.UpdateWorldRotation();
        }

        public bool IsDescendantOf(GameObject target)
        {
            GameObject current = Parent;

            while (current is not null)
            {
                if (current == target) return true;
                current = current.Parent;
            }

            return false;
        }

        public void RemoveChild(GameObject child)
        {
            if (child is null || child.Parent != this) return;

            _children.Remove(child);
            child.Parent = null;

            child._localPosition = child._position;
            child._localRotation = child._rotation;
        }

        public void SetParent(GameObject newParent) => newParent?.AddChild(this);
        public void DetachFromParent() => Parent?.RemoveChild(this);

        public void RemoveParent()
        {
            if (Parent is null) return;
            Vector2 worldPos = _position;
            double worldRot = _rotation;

            DetachFromParent();

            _localPosition = worldPos;
            _localRotation = worldRot;
            _position = worldPos;
            _rotation = worldRot;
        }

        public void Awake()
        {
            if (_awoken) return;
            _awoken = true;

            OnAwake();
        }

        protected virtual void OnAwake() { }

        public void Destroy()
        {
            DestroyChildren();
            DetachFromParent();

            foreach (var component in _components)
                component.Dispose();

            _components.Clear();

            GameObjectFactory.Instance.Delete(this);
        }

        public void Destroy(double delay) => FunctionTimer.Start(Destroy, delay);

        public T AddComponent<T>() where T : Component, new()
        {
            var component = new T() { gameObject = this };
            component.Awake();
            _components.Add(component);

            return component;
        }

        public T GetComponent<T>() where T : Component =>
             _components.OfType<T>().FirstOrDefault();

        public bool TryGetComponent<T>(out T component) where T : Component
        {
            component = GetComponent<T>();
            return component is not null;
        }

        public bool RemoveComponent<T>() where T : Component
        {
            var component = GetComponent<T>();
            if (component is null) return false;

            component.Dispose();
            _components.Remove(component);
            return true;
        }

        public List<Component> GetComponents() => _components;

        public List<T> GetComponents<T>() where T : Component =>
            _components.OfType<T>().ToList();

        void DestroyChildren()
        {
            var childrenCopy = _children.ToList();
            foreach (var child in childrenCopy)
                child.Destroy();

            _children.Clear();
        }

        public GameObject FindChild(Predicate<GameObject> predicate) =>
             _children.FirstOrDefault(c => predicate?.Invoke(c) ?? false);

        public List<GameObject> FindChildren(Predicate<GameObject> predicate)
        {
            if (predicate is null) return [];
            return _children.Where(c => predicate(c)).ToList();
        }

        public T FindChildOfType<T>() where T : GameObject =>
             _children.OfType<T>().FirstOrDefault();

        public List<T> FindChildrenOfType<T>() where T : GameObject =>
             _children.OfType<T>().ToList();


        public int GetChildCount() => _children.Count;
        public bool HasChildren() => _children.Count > 0;

        public virtual void OnCollisionEnter(Collider other) { }
        public virtual void OnCollisionStay(Collider other) { } 
        public virtual void OnCollisionExit(Collider other) { }
    }
}

