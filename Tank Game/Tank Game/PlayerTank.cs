namespace Tank_Game
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media;
    using System.Windows.Shapes;

    internal class PlayerTank : IUpdatable
    {
        PlayerInput _input = new PlayerInput();
        Canvas _gameCanvas = MainWindow.Instance.GameCanvas;
        public readonly Rectangle body;
        public readonly Rectangle turret;

        Vector2 _position;

        public double Speed = 1; // pixels per second
        public double RotationAngle { get; private set; }

        public PlayerTank()
        {
            body = new Rectangle
            {
                Width = 40,
                Height = 40,
                Fill = Brushes.Green
            };

            turret = new Rectangle
            {
                Width = 40,
                Height = 10,
                Fill = Brushes.DarkGreen,
                RenderTransformOrigin = new Point(0, 0.5)
            };

            _position = new Vector2(500, 350);

            _gameCanvas.Children.Add(body);
            _gameCanvas.Children.Add(turret);
        }

        public void Move(Vector2 direction)
        {
            if (direction == Vector2.Zero) return;
            _position += direction * Speed * GameLoop.DeltaTime;  
        }

        public void Update()
        {
            Canvas.SetLeft(body, _position.x);
            Canvas.SetTop(body, _position.y);

            Canvas.SetLeft(turret, _position.x + body.Width / 2);
            Canvas.SetTop(turret, _position.y + body.Height / 2 - turret.Height / 2);

            turret.RenderTransform = new RotateTransform(RotationAngle * 180 / Math.PI);
        }

        public void AimAt(Point mousePos)
        {
            double centerX = _position.x + body.Width / 2;
            double centerY = _position.y + body.Height / 2;

            double dx = mousePos.X - centerX;
            double dy = mousePos.Y - centerY;

            RotationAngle = Math.Atan2(dy, dx);
        }

        public Point GetGunPosition()
        {
            double gx = _position.x + 20 + Math.Cos(RotationAngle) * 30;
            double gy = _position.y + 20 + Math.Sin(RotationAngle) * 30;
            return new Point(gx, gy);
        }
    }
}
