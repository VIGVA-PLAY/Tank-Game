namespace Tank_Game
{
    internal class GameObject
    {
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public double RotationAngle { get; set; } // in degrees
        public GameObject(Vector2 position, Vector2 velocity, double rotationAngle)
        {
            Position = position;
            Velocity = velocity;
            RotationAngle = rotationAngle;
        }
    }
}
