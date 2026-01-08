namespace Tank_Game
{
    using System.Windows.Media;

    internal class PlayerTank : GameObject, IUpdate
    {
        public PlayerInput Input { get; private set; }
        public Turret Turret { get; private set; }
        public EllipseRenderer Ellipse { get; private set; }
        public CircleCollider Collider { get; private set; }

        public double Speed = 150; // pixels per second
        double _diameter = 40; 

        protected override void OnAwake()
        {
            Ellipse = AddComponent<EllipseRenderer>();
            Ellipse.Size = Vector2.One * _diameter;
            Ellipse.Pivot = Vector2.One / 2;
            Ellipse.Fill = Brushes.Blue;
            Ellipse.StrokeThickness = 2;
            Ellipse.Stroke = Brushes.DarkBlue;
            Ellipse.Layer = 1;

            Input = new PlayerInput();

            Turret = GameObjectFactory.Instance.Instantiate<Turret>();
            Turret.Anchor = this;
            Turret.BulletColor = Brushes.Blue;
            Turret.BulletSpeed = 600;
            Turret.BulletDiameter = 14;
            Turret.BulletType = BulletType.Player;

            Collider = AddComponent<CircleCollider>();
            Collider.Radius = _diameter / 2;
        }

        public void Update()
        {
            Move(Input.MoveDirection);
            Turret.AimAt(Input.MousePosition);

            if (Input.isFirePressed)
            {
                Turret.Shoot();
                Input.isFirePressed = false;
            }
        }

        public void Move(Vector2 direction)
        {
            if (direction == Vector2.Zero) return;
            Position += direction * Speed * GameLoop.DeltaTime;

            double clampedX = Math.Clamp(Position.x, 0, gameCanvas.Width);
            double clampedY = Math.Clamp(Position.y, 0, gameCanvas.Height);
            var clampedPosition = new Vector2(clampedX, clampedY);

            Position = clampedPosition;
        }
    }
}
