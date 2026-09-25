using DesignPatternLab.Characters.Bike;
using DesignPatternLab.Systems.PowerUps;

namespace DesignPatternLab.Systems.Replay.Commands
{
    // Both demo buttons and track pickups follow the existing recording path.
    public class ApplyPowerUp : Command
    {
        private readonly BikeController _bike;
        private readonly PowerUp _powerUp;
        private readonly Pickup _pickup;

        public ApplyPowerUp(BikeController bike, PowerUp powerUp, Pickup pickup = null)
        {
            _bike = bike;
            _powerUp = powerUp;
            _pickup = pickup;
        }

        public override void Execute()
        {
            if (!_bike || !_powerUp)
                return;

            _bike.Accept(_powerUp);
            if (_pickup)
                _pickup.Consume();
        }
    }
}
