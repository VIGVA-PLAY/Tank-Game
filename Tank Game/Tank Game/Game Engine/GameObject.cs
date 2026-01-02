namespace Tank_Game
{
    using System;

    internal abstract class GameObject 
    {
        Vector2 _position;
        double _rotation;
        Vector2 _localPosition;
        double _localRotation;

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                _renderer?.Update();

                if (_children.Any())
                {
                    foreach (var child in _children)
                        child.Position = Position + child.LocalPosition;
                }
            }
        }

        public double Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                _renderer?.Update();
            }
        }

        public Vector2 LocalPosition
        {
            get => _localPosition;
            set
            {
                _localPosition = value;
                if (Parent is not null)
                    Position = Parent.Position + _localPosition;
                else 
                    Position = _localPosition;
            }
        }

        readonly List<GameObject> _children = new();
        protected GameObject Parent { get; private set; }

        protected abstract Type RendererType { get; }
        IRenderer _renderer;

        protected GameObject() { }

        void CreateRenderer()
        {
            if (RendererType == null) return;

            _renderer = (IRenderer)Activator.CreateInstance(RendererType, this)!;
            _renderer.Update();
        }

        public virtual void Awake()
        {
            CreateRenderer();
        }

        public void Destroy()
        {
            GameObjectFactory.Instance.Dispose(this);
            _renderer.Dispose();
        }

        public void Destroy(int delay) => FunctionTimer.Start(Destroy, delay);

        public T GetRenderer<T>() where T : IRenderer =>
            (T)_renderer!;

        public void AddChild(GameObject go)
        {
            if (go is null)
                throw new ArgumentNullException(nameof(go));

            if (go == this)
                throw new InvalidOperationException("GameObject cannot be its own child.");

            if (IsDescendantOf(go))
                throw new InvalidOperationException("Cannot create cyclic hierarchy.");

            go.Parent?._children.Remove(go);

            go.Parent = this;
            go.Position = Position + go.LocalPosition;
            _children.Add(go);
        }

        public bool IsDescendantOf(GameObject target)
        {
            GameObject current = this;

            while (current != null)
            {
                if (current == target) return true;
                current = current.Parent;
            }

            return false;
        }

        public void RemoveParent()
        {
            if (Parent is not null)
            {
                Parent = null;
                _children.Remove(this);
            }
        }
    }
}
