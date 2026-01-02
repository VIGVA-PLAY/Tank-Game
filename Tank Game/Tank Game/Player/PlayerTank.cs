namespace Tank_Game
{
    using System;

    internal class PlayerTank : GameObject, IUpdatable
    {
        protected override Type RendererType => typeof(TankRenderer);

        public PlayerInput Input { get; private set; }
        public Turret Turret { get; private set; }
        public TankRenderer Renderer { get; private set; }

        public double Speed = 150; // pixels per second

        public override void Awake()
        {
            base.Awake();
            Renderer = GetRenderer<TankRenderer>();
            Input = new PlayerInput();
            Turret = GameObjectFactory.Instance.Instantiate<Turret>();
            AddChild(Turret);
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
