using System.Windows.Shapes;

namespace Tank_Game
{
    internal class Bullet : IUpdatable
    {
        public readonly Ellipse body;

        public Vector2 Position { get; private set; }

        public double Speed { get; private set; } // pixels per second
        public double Rotation { get; private set; } // radians

        public Bullet(Vector2 position, double rotation)
        {
            Position = position;
            Rotation = rotation;

            GameLoop.Instance.RegisterUpdatable(this);
        }

        public void Update()
        {
            Move();
        }

        void Move()
        {
            Position += Vector2.GetDirection(Rotation) * Speed * GameLoop.DeltaTime;
        }
    }
}
