namespace Tank_Game
{
    internal class Bullet : GameObject, IUpdatable
    {
        protected override Type RendererType => typeof(BulletRenderer);

        public double Speed = 300; // pixels per second

        public void Update()
        {
            Position += Vector2.GetDirection(Rotation) * Speed * GameLoop.DeltaTime;
        }
    }
}
