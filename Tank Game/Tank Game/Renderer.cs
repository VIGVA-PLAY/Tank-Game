using System.Windows.Controls;
using System.Windows.Shapes;

namespace Tank_Game
{
    internal abstract class Renderer<TGameObject, TShape> : IRenderer
        where TGameObject : GameObject
        where TShape : Shape, new()
    {
        readonly Canvas _gameCanvas = MainWindow.Instance.GameCanvas;
        public readonly TShape body;
        public readonly TGameObject gameObject;

        public Renderer(TGameObject gameObject)
        {
            this.gameObject = gameObject;
            body = new TShape();

            _gameCanvas.Children.Add(body);
        }

        public abstract void Update();

        public void Dispose()
        {
            if (body != null) 
                _gameCanvas.Children.Remove(body);
        }
    }
}
