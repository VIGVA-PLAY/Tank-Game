
using System.Windows;

namespace Tank_Game
{
    internal class Turret : GameObject
    {
        protected override Type RendererType => typeof(TurretRenderer);
        public TurretRenderer Renderer { get; private set; }
        double _turretLength;

        public override void Awake()
        {
            base.Awake();
            Renderer = GetRenderer<TurretRenderer>();
            _turretLength = Renderer.body.Width;
        }

        public void AimAt(Point mousePos)
        {
            double dx = mousePos.X - Position.x;
            double dy = mousePos.Y - Position.y;

            Rotation = Math.Atan2(dy, dx);
        }

        public void Shoot()
        {
            var bullet = GameObjectFactory.Instance.Instantiate<Bullet>(GetBulletSpawnPosition(), Rotation);
            bullet.Destroy(10);  
        }

        Vector2 GetBulletSpawnPosition()
        {
            Vector2 direction = Vector2.GetDirection(Rotation);
            Vector2 spawnPosition = direction * _turretLength + Position;

            return spawnPosition;
        }
    }
}
