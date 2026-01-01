using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tank_Game
{
    internal class TurretRenderer : Renderer<Turret, Rectangle>
    {
        public TurretRenderer(Turret gameObject) : base(gameObject)
        {
            body.Width = 40;
            body.Height = 10;
            body.Fill = Brushes.DarkGreen;
            body.RenderTransformOrigin = new Point(0, 0.5);
            Canvas.SetZIndex(body, ZIndex = 1);
            pivot = new Vector2(0, body.Height / 2);
        }

        public override void Update()
        {
            base.Update();

            body.RenderTransform = new RotateTransform(
                gameObject.Rotation * 180 / Math.PI);
        }
    }
}
