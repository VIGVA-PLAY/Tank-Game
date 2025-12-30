namespace Tank_Game
{
    using System;

    internal abstract class GameObject : IDisposable
    {
        public Vector2 Position;
        public double Rotation; // in radians

        protected GameObject()
        {
            
        }

        public void Dispose()
        {
           // if (renderer is IDisposable disposable)
                //disposable.Dispose();
        }
    }
}
