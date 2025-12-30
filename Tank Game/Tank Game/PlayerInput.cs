namespace Tank_Game
{
    internal class PlayerInput : IUpdatable
    {
        public bool MoveForward { get; set; }
        public bool MoveBackward { get; set; }
        public bool TurnLeft { get; set; }
        public bool TurnRight { get; set; }
        public bool Fire { get; set; }

        public void Update()
        {

        }
    }
}
