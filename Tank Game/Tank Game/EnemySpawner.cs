using System.Windows.Controls;
using Tank_Game.Enemies;

namespace Tank_Game
{
    internal class EnemySpawner
    {
        readonly Canvas gameCanvas = MainWindow.Instance.GameCanvas;

        //Singleton
        static readonly Lazy<EnemySpawner> _instance = new(() => new EnemySpawner());
        public static EnemySpawner Instance => _instance.Value;
        EnemySpawner() { }

        static Random _random;

        Function.Timer _spawningTimer;
        double _spawnDelay = 3;

        public void StartSpawning()
        {
            _random = new Random();
            _spawningTimer = Function.StartRepeating(Spawn, _spawnDelay);
        }

        public void StopSpawning()
        {
            _spawningTimer?.Dispose();
            _spawningTimer = null;
        }

        public void Spawn()
        {
            GameObjectFactory.Instance.Instantiate<MiniTank>(GetRandomPosition());
        }

        Vector2 GetRandomPosition()
        {
            double rndX = _random.NextDouble() * gameCanvas.Width;
            double rndY = _random.NextDouble() * gameCanvas.Height;

            Vector2 spawnPosition = new Vector2(rndX, rndY);

            return spawnPosition;
        }
    }
}
