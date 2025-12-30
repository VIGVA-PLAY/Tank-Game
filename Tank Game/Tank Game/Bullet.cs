using System.Windows.Media;
using System.Windows.Shapes;

namespace Tank_Game
{
    internal class Bullet
    {
        public readonly Ellipse body;

        double x;
        double y;

        public double Speed { get; private set; } // pixels per second
        public double RotationAngle { get; private set; }


        public Bullet(Vector2 pos)
        {
            this.x = x;
            this.y = y;
            //Speed = speed;
        }
    }
}
