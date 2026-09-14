using DesignPatternLab.Characters.Bike;

namespace DesignPatternLab.Systems.Replay.Commands
{
    public class ToggleTurbo : Command
    {
        private readonly BikeController _controller;

        public ToggleTurbo(BikeController controller)
        {
            _controller = controller;
        }

        public override void Execute()
        {
            _controller.ToggleTurbo();
        }
    }
}
