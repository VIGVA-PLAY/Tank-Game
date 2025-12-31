namespace Tank_Game
{
    using System;

    internal abstract class GameObject : IDisposable
    {
        Vector2 _position;
        double _rotation;

        public Vector2 Position
        {
            get => _position;
            set
            {
                _position = value;
                Renderer?.Update();
            }
        }

        public double Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                Renderer?.Update();
            }
        }

        public abstract IRenderer Renderer { get; protected set; }

        protected GameObject() { }

        public abstract void Awake();

        public void Dispose()
        {
            Renderer.Dispose();
        }
    }
}
