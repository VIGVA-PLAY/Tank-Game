namespace Tank_Game
{
    internal class Bullet : GameObject, IUpdate
    {
        public double Speed = 300; // pixels per second
        public EllipseRenderer EllipseRenderer { get; private set; }

        protected override void OnAwake()
        {
            EllipseRenderer = AddComponent<EllipseRenderer>();
            EllipseRenderer.Size = new Vector2(10, 10);
        }

        public void Update()
        {
            Position += Vector2.GetDirection(Rotation) * Speed * GameLoop.DeltaTime;
        }
    }
}
