using System.Windows.Media;

namespace Tank_Game
{
    internal class Turret : GameObject, ILateUpdate
    {
        public GameObject Anchor;
        public RectRenderer Rect { get; private set; }

        double _turretLength;
        public Brush BulletColor;
        public double BulletSpeed = 300;
        public double BulletDiameter = 10;
        public double BulletDamage;
        public BulletType BulletType;

        protected override void OnAwake()
        {
            Rect = AddComponent<RectRenderer>();
            Rect.Width = 40;
            Rect.Height = 15;
            Rect.Pivot = new Vector2(0, 0.5);
            Rect.Layer = 0;

            _turretLength = Rect.Width;
        }

        public void LateUpdate()
        {
            Position = Anchor.Position;
        }

        public void AimAt(Vector2 mousePos)
        {
            double dx = mousePos.x - Position.x;
            double dy = mousePos.y - Position.y;

            Rotation = Math.Atan2(dy, dx);
        }

        public void Shoot()
        {
            var bullet = GameObjectFactory.Instance.Instantiate<Bullet>(GetBulletSpawnPosition(), Rotation);
            bullet.Speed = BulletSpeed;
            bullet.Color = BulletColor;
            bullet.Diameter = BulletDiameter;
            bullet.Type = BulletType;

            bullet.Destroy(10);
        }

        Vector2 GetBulletSpawnPosition()
        {
            Vector2 direction = Vector2.GetDirection(Rotation);
            Vector2 spawnPosition = direction * (_turretLength - BulletDiameter / 2) + Position;

            return spawnPosition;
        }
    }
}
