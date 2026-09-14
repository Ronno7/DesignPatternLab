using DesignPatternLab.Characters.Bike;

namespace DesignPatternLab.Systems.Replay.Commands
{
    public class TurnRight : Command
    {
        private readonly BikeController _controller;

        public TurnRight(BikeController controller)
        {
            _controller = controller;
        }

        public override void Execute()
        {
            _controller.Turn(Direction.Right);
        }
    }
}
