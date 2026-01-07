using System.Windows.Media;
using System.Windows.Shapes;

namespace Tank_Game
{
    internal sealed class RectRenderer : Renderer
    {
        public Vector2 Size 
        {
            get => new(Width, Width);
            set
            {
                Width = value.x;
                Height = value.y;
            }
        }

        public double Width
        {
            get => body.Width;
            set => body.Width = value <= 0 ? 0 : value;
        }

        public double Height
        {
            get => body.Height;
            set => body.Height = value <= 0 ? 0 : value;
        }

        public RectRenderer()
        {
            body = new Rectangle();

            body.Width = 50;
            body.Height = 50;
            body.Fill = Brushes.Gray;

            gameCanvas.Children.Add(body);
        }

        public Vector2 GetCenter() =>
            new Vector2(body.Width, body.Height) / 2;
    }
}
