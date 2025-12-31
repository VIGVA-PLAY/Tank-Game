using System.Windows;
using System.Windows.Input;

namespace Tank_Game
{
    public partial class MainWindow : Window
    {
        PlayerTank _player;
        GameObjectFactory _gameObjectFactory = new GameObjectFactory();

        public static MainWindow Instance => (Application.Current.MainWindow as MainWindow)!;

        public MainWindow()
        {
            InitializeComponent();
            GameCanvas.Focus();

            _player = _gameObjectFactory.Instantiate<PlayerTank>(new Vector2(500, 350));

            GameLoop.Instance.Run();
        }

        void Window_KeyDown(object sender, KeyEventArgs e)
        {
            _player.input.KeyDown(e.Key);
        }

        void Window_KeyUp(object sender, KeyEventArgs e)
        {
            _player.input.KeyUp(e.Key);
        }

        void Window_MouseMove(object sender, MouseEventArgs e)
        {
            _player.input.MousePosition = e.GetPosition(GameCanvas);
        }

        void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                _player.input.MouseDown();
        }
    }
}