namespace Tank_Game
{
    using System;

    internal class PlayerTank : GameObject, IUpdatable
    {
        protected override Type RendererType => typeof(TankRenderer);
        public readonly PlayerInput input = new PlayerInput();
        public readonly Turret turret = GameObjectFactory.Instance.Instantiate<Turret>();
        public TankRenderer Renderer { get; private set; }

        public double Speed = 150; // pixels per second

        public override void Awake()
        {
            base.Awake();
            Renderer = GetRenderer<TankRenderer>();
            AddChild(turret);
        }

        public void Update()
        {
            Move(input.MoveDirection);
            turret.AimAt(input.MousePosition);

            if (input.isFirePressed)
            {
                turret.Shoot();
                input.isFirePressed = false;
            } 
        }

        public void Move(Vector2 direction)
        {
            if (direction == Vector2.Zero) return;
            Position += direction * Speed * GameLoop.DeltaTime;
        }
    }
}
