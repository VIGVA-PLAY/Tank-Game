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
            pivot = new Vector2(body.Width / 2, body.Height / 2);
        }
    }
}
