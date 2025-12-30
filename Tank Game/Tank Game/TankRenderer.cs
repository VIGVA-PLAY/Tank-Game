using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;   

namespace Tank_Game
{
    internal class TankRenderer : IRenderer
    {
        Canvas _gameCanvas = MainWindow.Instance.GameCanvas;
        public readonly Rectangle body;
        public readonly GameObject tank;

        public TankRenderer()
        {
            body = new Rectangle
            {
                Width = 40,
                Height = 40,
                Fill = Brushes.Green
            };

            _gameCanvas.Children.Add(body);
        }

        public void Draw()
        {
            Canvas.SetLeft(body, tank.Position.x);
            Canvas.SetTop(body, tank.Position.y);
        }

        public void Dispose()
        {
            if (body != null) _gameCanvas.Children.Remove(body);
        }
    }
}
