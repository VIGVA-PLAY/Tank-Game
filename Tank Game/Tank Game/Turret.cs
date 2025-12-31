namespace Tank_Game
{
    internal class Turret : GameObject, IUpdatable
    {
        public override void Awake()
        {
            Renderer = new TurretRenderer(this);
            Renderer.Update();
        }

        public void Update()
        {

        }
    }
}
