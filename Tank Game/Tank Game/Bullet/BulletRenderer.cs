using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Tank_Game
{
    internal class BulletRenderer : Renderer<Bullet, Ellipse>
    {
        public BulletRenderer(Bullet gameObject) : base(gameObject)
        {
            body.Width = 10;
            body.Height = 10;
            body.Stroke = Brushes.Black;
            body.StrokeThickness = 1;
            body.Fill = Brushes.Blue;
            Canvas.SetZIndex(body, ZIndex = 2);
            
            pivot = new Vector2(body.Width, body.Height) / 2;
        }
    }
}
