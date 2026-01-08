using System.Windows.Media;
using Tank_Game.Enemies;

namespace Tank_Game
{
    public enum BulletType
    {
        Enemy,
        Player
    }

    internal class Bullet : GameObject, IUpdate
    {
        public double Speed = 300; // pixels per second
        public EllipseRenderer Ellipse { get; private set; }
        public CircleCollider Collider { get; private set; }

        public BulletType Type;

        public double Diameter
        {
            get => Ellipse.Width;
            set
            {
                Ellipse.Size = Vector2.One * value;
                if (Collider is not null) Collider.Radius = value / 2;
            }
        }

        public Brush Color
        {
            get => Ellipse.Fill;
            set => Ellipse.Fill = value;
        }

        protected override void OnAwake()
        {
            Ellipse = AddComponent<EllipseRenderer>();
            Ellipse.Size = Vector2.One * Diameter;
            Ellipse.Pivot = Vector2.One / 2;
            Ellipse.Layer = -1;

            Collider = AddComponent<CircleCollider>();
            Collider.Radius = Diameter / 2;
        }

        public void Update()
        {
            Position += Vector2.GetDirection(Rotation) * Speed * GameLoop.DeltaTime;
        }

        public override void OnCollisionEnter(Collider other)
        {
            if (other.gameObject is Bullet otherbullet)
            {
                if (otherbullet.Type != Type) Destroy(); 
            }
            else if (other.gameObject is PlayerTank player)
            {
                if (Type == BulletType.Enemy) Destroy();
            }
            else if (other.gameObject is MiniTank enemy)
            {
                if (Type == BulletType.Player) enemy.ApplyDamage(100);
            }
        }
    }
}
