namespace Tank_Game
{
    using System.Windows;

    internal class PlayerTank : GameObject, IUpdatable
    {
        public readonly PlayerInput input = new PlayerInput();
        public readonly Turret turret = new Turret();
        public override IRenderer Renderer { get; protected set; }

        public override void Awake()
        {
            Renderer = new TankRenderer(this);
            Renderer.Update();
        }

        public double Speed = 150; // pixels per second

        public void Move(Vector2 direction)
        {
            if (direction == Vector2.Zero) return;
            Position += direction * Speed * GameLoop.DeltaTime;
        }

        public void Update()
        { 
            Move(input.MoveDirection);
            AimAt(input.MousePosition);
        }

        public void AimAt(Point mousePos)
        {
            //double centerX = Position.x + body.Width / 2;
            //double centerY = Position.y + body.Height / 2;

            //double dx = mousePos.X - centerX;
            //double dy = mousePos.Y - centerY;

            //turret.Rotation = Math.Atan2(dy, dx);
        }


        //public void Shoot()
        //{
        //    Point gunPos = GetGunPosition();
        //    var bullet = new Bullet(new Vector2(gunPos.X, gunPos.Y), turret.Rotation);
        //    //GameLoop.Instance.AddEntity(bullet);
        //}

        //public Point GetGunPosition()
        //{
        //    double gx = Position.x + 20 + Math.Cos(turret.Rotation) * 30;
        //    double gy = Position.y + 20 + Math.Sin(turret.Rotation) * 30;
        //    return new Point(gx, gy);
        //}
    }
}
