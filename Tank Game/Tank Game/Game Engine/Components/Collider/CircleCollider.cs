namespace Tank_Game
{
    internal sealed class CircleCollider : Collider
    {
        public double Radius { get; set; }

        public override bool Intersects(Collider other)
        {
            if (other is CircleCollider circleCollider)
            {
                double radiusSum = Radius + circleCollider.Radius;
                return (Position - circleCollider.Position).sqrMagnitude <= radiusSum * radiusSum;
            }

            return false;
        }
    }
}
