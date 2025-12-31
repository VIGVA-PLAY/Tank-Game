using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tank_Game
{
    internal class TankRenderer : Renderer<PlayerTank, Rectangle>
    {
        public TankRenderer(PlayerTank gameObject) : base(gameObject)
        {
            body.Width = 40;
            body.Height = 40;
            body.Fill = Brushes.Green;
        }

        public override void Update()
        {
            Canvas.SetLeft(body, gameObject.Position.x);
            Canvas.SetTop(body, gameObject.Position.y);
        }
    }
}
