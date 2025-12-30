namespace Tank_Game
{
    internal struct Vector2 : IEquatable<Vector2>
    {
        public double x;
        public double y;

        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector2 normalized {
            get
            {
                double length = Math.Sqrt(x * x + y * y);
                if (length > Epsilon)
                    return new Vector2(x / length, y / length);

                return new Vector2(0, 0);
            }
        }

        public static Vector2 Zero => new(0, 0);
        public static Vector2 One => new(1, 1);
        public static Vector2 UnitX => new(1, 0);
        public static Vector2 UnitY => new(0, 1);

        #region Operators
        public static Vector2 operator +(Vector2 a, Vector2 b) =>
            new(a.x + b.x, a.y + b.y);

        public static Vector2 operator -(Vector2 a, Vector2 b) =>
            new(a.x - b.x, a.y - b.y);

        public static Vector2 operator *(Vector2 a, double scalar) =>
            new(a.x * scalar, a.y * scalar);

        public static Vector2 operator /(Vector2 a, double scalar)
        {
            if (scalar == 0)
                throw new DivideByZeroException();

            return new(a.x / scalar, a.y / scalar);
        }

        public static bool operator ==(Vector2 a, Vector2 b) =>
            a.Equals(b);

        public static bool operator !=(Vector2 a, Vector2 b) =>
            !a.Equals(b);
        #endregion

        const double Epsilon = 1e-9;

        public bool Equals(Vector2 other) =>
            Math.Abs(x - other.x) < Epsilon &&
            Math.Abs(y - other.y) < Epsilon;

        public override bool Equals(object? obj) =>
            obj is Vector2 v && Equals(v);

        public override int GetHashCode() =>
            HashCode.Combine(x, y);

        public override string ToString() => $"({x}, {y})";

        public void Normalize()
        {
            double length = Math.Sqrt(x * x + y * y);
            if (length > Epsilon)
            {
                x /= length;
                y /= length;
            }
        }
    }
}
