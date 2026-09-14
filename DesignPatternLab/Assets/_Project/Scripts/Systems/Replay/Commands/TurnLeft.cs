using DesignPatternLab.Characters.Bike;

namespace DesignPatternLab.Systems.Replay.Commands
{
    public class TurnLeft : Command
    {
        private readonly BikeController _controller;

        public TurnLeft(BikeController controller)
        {
            _controller = controller;
        }

        public override void Execute()
        {
            _controller.Turn(Direction.Left);
        }
    }
}
