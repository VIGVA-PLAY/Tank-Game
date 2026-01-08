using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tank_Game
{
    internal abstract class Renderer : Component
    {
        protected readonly Canvas gameCanvas = MainWindow.Instance.GameCanvas;
        protected Shape body;
        Vector2 _pivot;

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
            set
            {
                body.Width = value <= 0 ? 0 : value;
                UpdatePosition();
            }
        }

        public double Height
        {
            get => body.Height;
            set
            {
                body.Height = value <= 0 ? 0 : value;
                UpdatePosition();
            }
        }

        public Vector2 Pivot
        {
            get => _pivot;
            set
            {
                if (_pivot == value) return;
                _pivot = value;
                body.RenderTransformOrigin = new Point(_pivot.x, _pivot.y);
                UpdatePosition();
            }
        }

        public int Layer
        {
            get => Canvas.GetZIndex(body);
            set => Canvas.SetZIndex(body, value);
        }

        public Brush Fill
        {
            get => body.Fill;
            set => body.Fill = value;
        }

        public double StrokeThickness
        {
            get => body.StrokeThickness;
            set => body.StrokeThickness = value <= 0 ? 0 : value;
        }

        public Brush Stroke
        {
            get => body.Stroke;
            set => body.Stroke = value;
        }

        protected override void OnAwake()
        {
            Update();
            gameObject.OnTransform += Update;
        }

        protected override void OnDispose()
        {
            gameObject.OnTransform -= Update;

            if (body != null)
                gameCanvas.Children.Remove(body);
        }

        public void Update()
        {
            UpdatePosition();
            UpdateRotation();
        }

        protected void UpdatePosition()
        {
            Canvas.SetLeft(body, gameObject.Position.x - _pivot.x * body.Width);
            Canvas.SetTop(body, gameObject.Position.y - _pivot.y * body.Height);
        }

        void UpdateRotation()
        {
            body.RenderTransform = new RotateTransform(
               gameObject.Rotation * 180 / Math.PI);
        }
    }
}
