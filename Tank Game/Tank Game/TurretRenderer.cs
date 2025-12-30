using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tank_Game
{
    internal class TurretRenderer : IRenderer
    {
        private readonly Canvas _gameCanvas = MainWindow.Instance.GameCanvas;
        public readonly Rectangle body;
        public readonly GameObject turret;
        public readonly PlayerTank tank;

        public TurretRenderer(PlayerTank tank, GameObject turret)
        {
            this.tank = tank ?? throw new ArgumentNullException(nameof(tank));
            this.turret = turret ?? throw new ArgumentNullException(nameof(turret));

            body = new Rectangle
            {
                Width = 40,
                Height = 10,
                Fill = Brushes.DarkGreen,
                RenderTransformOrigin = new Point(0, 0.5)
            };

            _gameCanvas.Children.Add(body);
        }

        public void Draw()
        {
            Canvas.SetLeft(body, tank.Position.x + 20);
            Canvas.SetTop(body, tank.Position.y + 15);

            body.RenderTransform =
                new RotateTransform(turret.Rotation * 180 / Math.PI);
        }

        public void Dispose()
        {
            if (body != null) _gameCanvas.Children.Remove(body);
        }
    }
}
