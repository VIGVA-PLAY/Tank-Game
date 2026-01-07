using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.Windows.Ink;

namespace Tank_Game
{
    internal abstract class Renderer : Component
    {
        protected readonly Canvas gameCanvas = MainWindow.Instance.GameCanvas;
        protected Shape body;
        Vector2 _pivot;
        public Vector2 Pivot 
        {
            get => _pivot;
            set
            {
                if(value == _pivot) return;
                _pivot = value;
                if (body.Height * body.Width <= 0) return;
                body.RenderTransformOrigin = new Point(_pivot.x / body.Width, _pivot.y / body.Height);
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

        void UpdatePosition()
        {
            Canvas.SetLeft(body, gameObject.Position.x - _pivot.x);
            Canvas.SetTop(body, gameObject.Position.y - _pivot.y);
        }

        void UpdateRotation()
        {
            body.RenderTransform = new RotateTransform(
               gameObject.Rotation * 180 / Math.PI);
        }
    }
}
