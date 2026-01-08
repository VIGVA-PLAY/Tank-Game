using System.Windows;
using System.Windows.Input;
using Tank_Game.Enemies;

namespace Tank_Game
{
    public partial class MainWindow : Window
    {
        public static MainWindow Instance => (Application.Current.MainWindow as MainWindow)!;
        PlayerTank _player;
        GameObjectFactory _factory = GameObjectFactory.Instance;
        GameLoop GameLoop => GameLoop.Instance;
        EnemySpawner _enemySpawner;

        public MainWindow()
        {
            InitializeComponent();
            GameCanvas.Focus();
            GameLoop.Run();
            _enemySpawner = EnemySpawner.Instance;

            _player = _factory.Instantiate<PlayerTank>(GameCanvas.Width / 2, GameCanvas.Height / 2);

            _enemySpawner.StartSpawning();
        }

        void Window_KeyDown(object sender, KeyEventArgs e)
        {
            _player?.Input.KeyDown(e.Key);

            //if (e.Key == Key.Right) _player.Rotation += Math.PI / 12;
            //if (e.Key == Key.Left) _player.Rotation -= Math.PI / 12;
            //if (e.Key == Key.Up) _player.Turret.LocalPosition += Vector2.UnitX * 10;
            //if (e.Key == Key.Down) _player.Turret.LocalPosition -= Vector2.UnitX * 10;
        }

        void Window_KeyUp(object sender, KeyEventArgs e)
        {
            _player?.Input.KeyUp(e.Key);
        }

        void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if(_player == null) return;
            var mousePoint = e.GetPosition(GameCanvas);
            _player.Input.MousePosition = new Vector2(mousePoint.X, mousePoint.Y);
        }

        void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                _player?.Input.MouseDown();
        }
    }
}