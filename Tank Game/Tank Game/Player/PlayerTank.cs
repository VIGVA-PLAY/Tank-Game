namespace Tank_Game
{
    using System.Windows.Media;
    using System.Windows.Media.Effects;
    using Tank_Game;

    internal class PlayerTank : GameObject, IUpdate
    {
        public PlayerInput Input { get; private set; }
        public Turret Turret { get; private set; }
        public EllipseRenderer Ellipse { get; private set; }

        public double Speed = 150; // pixels per second

        protected override void OnAwake()
        {
            Ellipse = AddComponent<EllipseRenderer>();
            Ellipse.Size = new Vector2(40, 40);
            Ellipse.Pivot = Ellipse.GetCenter();
            Ellipse.Fill = Brushes.Blue;
            Ellipse.Layer = 1;

            Input = new PlayerInput();

            Turret = GameObjectFactory.Instance.Instantiate<Turret>();
            Turret.Anchor = this;
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
        }
    }
}
