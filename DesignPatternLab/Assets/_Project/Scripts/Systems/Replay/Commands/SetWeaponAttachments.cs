using DesignPatternLab.Characters.Bike;
using DesignPatternLab.Systems.Weapons;

namespace DesignPatternLab.Systems.Replay.Commands
{
    public class SetWeaponAttachments : Command
    {
        private readonly BikeWeapon _weapon;
        private readonly WeaponAttachment _main, _secondary;

        public SetWeaponAttachments(BikeWeapon weapon, WeaponAttachment main, WeaponAttachment secondary)
        {
            _weapon = weapon;
            _main = main;
            _secondary = secondary;
        }

        public override void Execute() => _weapon.Equip(_main, _secondary);
    }
}
