using DesignPatternLab.Characters.Bike;

namespace DesignPatternLab.Systems.Replay.Commands
{
    public class ToggleWeaponFire : Command
    {
        private readonly BikeWeapon _weapon;
        public ToggleWeaponFire(BikeWeapon weapon) => _weapon = weapon;
        public override void Execute() => _weapon.ToggleFire();
    }
}
