using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Tank_Game
{
    public partial class MainWindow : Window
    {
        GameLoop _gameLoop = new GameLoop();
        PlayerTank _player;

        bool up, down, left, right;
        Point _mousePosition;
        Vector2 _moveDirection = Vector2.Zero;

        public static MainWindow Instance => (Application.Current.MainWindow as MainWindow)!;

        public MainWindow()
        {
            InitializeComponent();
            GameCanvas.Focus();

            _player = new PlayerTank();
            _gameLoop.RegisterUpdatable(_player);
            _gameLoop.Start();
        }

        //void GameLoop(object sender, EventArgs e)
        //{
        //    _moveDirection = Vector2.Zero;

        //    if (up) _moveDirection.y = -1;
        //    if (down) _moveDirection.y = 1;
        //    if (left) _moveDirection.x = -1;
        //    if (right) _moveDirection.x = 1;

        //   _moveDirection.Normalize();

        //    _player.Move(_moveDirection * _deltaTime);
        //}

        void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W) up = true;
            if (e.Key == Key.S) down = true;
            if (e.Key == Key.A) left = true;
            if (e.Key == Key.D) right = true;

            PlayerInput input = new PlayerInput
            {
                MoveForward = up,
                MoveBackward = down,
                TurnLeft = left,
                TurnRight = right
            };
        }

        void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W) up = false;
            if (e.Key == Key.S) down = false;
            if (e.Key == Key.A) left = false;
            if (e.Key == Key.D) right = false;
        }

        void Window_MouseMove(object sender, MouseEventArgs e)
        {
            _mousePosition = e.GetPosition(GameCanvas);
            _player.AimAt(_mousePosition);
        }

        void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}