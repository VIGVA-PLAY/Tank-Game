using System.Windows;
using System.Windows.Input;

namespace Tank_Game
{
    public partial class MainWindow : Window
    {
        PlayerTank _player;
        GameObjectFactory _factory = GameObjectFactory.Instance;
        GameLoop _gameLoop => GameLoop.Instance;

        public static MainWindow Instance => (Application.Current.MainWindow as MainWindow)!;

        public MainWindow()
        {
            InitializeComponent();
            GameCanvas.Focus();
            _gameLoop.Run();

            _player = _factory.Instantiate<PlayerTank>(500, 350);
        }

        void Window_KeyDown(object sender, KeyEventArgs e)
        {
            _player?.Input.KeyDown(e.Key);
        }

        void Window_KeyUp(object sender, KeyEventArgs e)
        {
            _player?.Input.KeyUp(e.Key);
        }

        void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if(_player == null) return;
            _player.Input.MousePosition = e.GetPosition(GameCanvas);
        }

        void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                _player?.Input.MouseDown();
        }
    }
}