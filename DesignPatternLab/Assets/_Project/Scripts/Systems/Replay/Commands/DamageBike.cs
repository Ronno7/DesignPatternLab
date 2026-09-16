using DesignPatternLab.Characters.Bike;

namespace DesignPatternLab.Systems.Replay.Commands
{
    // Record the Chapter 9 test input because damage also changes Turbo and health.
    public class DamageBike : Command
    {
        private readonly BikeController _controller;
        private readonly float _amount;

        public DamageBike(BikeController controller, float amount)
        {
            _controller = controller;
            _amount = amount;
        }

        public override void Execute()
        {
            _controller.TakeDamage(_amount);
        }
    }
}
