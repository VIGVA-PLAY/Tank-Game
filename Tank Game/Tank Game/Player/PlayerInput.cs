using System.Windows;
using System.Windows.Input;

namespace Tank_Game
{
    internal class PlayerInput
    {
        public Vector2 MousePosition { get; set; }
        public bool isFirePressed;

        readonly HashSet<Key> _keys = new();

        public Vector2 MoveDirection
        {
            get
            {
                Vector2 dir = Vector2.Zero;

                if (_keys.Contains(Key.W)) dir.y -= 1;
                if (_keys.Contains(Key.S)) dir.y += 1;
                if (_keys.Contains(Key.A)) dir.x -= 1;
                if (_keys.Contains(Key.D)) dir.x += 1;

                if (dir.sqrMagnitude > 0)
                    dir.Normalize();

                return dir;
            }
        }

        public void KeyDown(Key key) => _keys.Add(key);
        public void KeyUp(Key key) => _keys.Remove(key);


        public void MouseDown() => isFirePressed = true;
    }
}
