using System.Windows.Media;

namespace Tank_Game.Enemies
{
    internal class MiniTank : GameObject, IUpdate
    {
        public Turret Turret { get; private set; }
        public EllipseRenderer Ellipse { get; private set; }
        public CircleCollider Collider { get; private set; }
        Random _random = new Random();

        PlayerTank _player;
        Vector2 _destination;
        double _maxDestinationDistance = 100;
        double _destinationChangeDelay = 2;


        public double Health { get; private set; } = 100;
        public double Speed = 60; // pixels per second
        double _diameter = 25;
        double shootDelay = 1;

        FunctionTimer.Timer _shootingTimer;
        FunctionTimer.Timer _destinationChangeTimer;

        protected override void OnAwake()
        {
            Ellipse = AddComponent<EllipseRenderer>();
            Ellipse.Size = Vector2.One * _diameter;
            Ellipse.Pivot = Vector2.One / 2;
            Ellipse.Fill = Brushes.Red;
            Ellipse.StrokeThickness = 2;
            Ellipse.Stroke = Brushes.DarkRed;
            Ellipse.Layer = 1;

            Turret = GameObjectFactory.Instance.Instantiate<Turret>();
            Turret.Anchor = this;
            Turret.Rect.Size = new Vector2(23, 7);
            Turret.BulletType = BulletType.Enemy;
            Turret.BulletDiameter = 7;
            Turret.BulletSpeed = 100;
            Turret.BulletColor = Brushes.Red;

            Collider = AddComponent<CircleCollider>();
            Collider.Radius = _diameter / 2;

            _player = GameObjectFactory.Instance.FindGameObject<PlayerTank>();

            _destination = Position;

            StartMoving();
            StartShooting();
        }

        public void Update()
        {
            HandleMovement();
            Turret.AimAt(_player.Position);
        }

        void HandleMovement()
        {
            if ((Position - _destination).sqrMagnitude < 1) return;

            var direction = (_destination - Position).normalized;
            Move(direction);
        }

        Vector2 GetRandomDestination()
        {
            double rndX = (_random.NextDouble() * 2) - 1;
            double rndY = (_random.NextDouble() * 2) - 1;

            Vector2 localDestination = new Vector2(rndX, rndY).normalized * _maxDestinationDistance;
            Vector2 destination = localDestination + Position;

            destination.x = Math.Clamp(destination.x, 0, gameCanvas.Width);
            destination.y = Math.Clamp(destination.y, 0, gameCanvas.Height);

            return destination;
        }

        public void Move(Vector2 direction)
        {
            if (direction == Vector2.Zero) return;
            Position += direction * Speed * GameLoop.DeltaTime;
        }

        public void StartShooting() => _shootingTimer = FunctionTimer.StartRepeating(Turret.Shoot, shootDelay);
        public void StopShooting() 
        {
            _shootingTimer?.Dispose();
            _shootingTimer = null;
        }
        public void StartMoving() 
        {
            _destinationChangeTimer = FunctionTimer.StartRepeating
            (
                () => _destination = GetRandomDestination(),
                _destinationChangeDelay
            );
        }

        public void StopMoving()
        {
            _destinationChangeTimer?.Dispose();
            _destinationChangeTimer = null;
        }

        public void ApplyDamage(double amount)
        {
            if(amount <= 0 || Health <= 0) return;  
            Health -= amount;

            if (Health <= 0) Die(); 
        }

        public void Die()
        {
            StopMoving();
            StopShooting();

            Turret.Destroy();
            Destroy();
        }
    }
}
