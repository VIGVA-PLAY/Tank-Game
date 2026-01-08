namespace Tank_Game
{
    internal abstract class Collider : Component
    {
        public Vector2 Offset;

        public Vector2 Position {  get; private set; }
        //public double Rotation { get; private set; }

        protected override void OnAwake()
        {
            Update();
            CollisionDetector.Instance.Register(this);
            gameObject.OnTransform += Update;
        }

        protected override void OnDispose()
        {
            CollisionDetector.Instance.Unregister(this);
            gameObject.OnTransform -= Update;
        }

        public abstract bool Intersects(Collider other);

        void Update() 
        {
            Position = gameObject.Position + Offset;
            //Rotation = gameObject.Rotation;
        }
    }
}
