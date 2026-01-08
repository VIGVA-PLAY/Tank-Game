using System.Windows.Media;
using System.Windows.Shapes;

namespace Tank_Game
{
    internal sealed class RectRenderer : Renderer
    {
        public RectRenderer()
        {
            body = new Rectangle();

            body.Width = 50;
            body.Height = 50;
            body.Fill = Brushes.Gray;

            gameCanvas.Children.Add(body);
        }
    }
}
