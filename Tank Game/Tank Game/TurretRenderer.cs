using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tank_Game
{
    internal class TurretRenderer : Renderer<Turret, Rectangle>
    {
        public TurretRenderer(Turret gameObject) : base(gameObject)
        {
            body.Width = 30;
            body.Height = 10;
            body.Fill = Brushes.DarkGray;
        }

        public override void Update()
        {
            Canvas.SetLeft(body, gameObject.Position.x - 20);
            Canvas.SetTop(body, gameObject.Position.y - 20);
        }
    }
}
