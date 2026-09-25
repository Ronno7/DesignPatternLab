using UnityEngine;

namespace DesignPatternLab.Systems.Weapons
{
    public class WeaponDecorator : IWeapon
    {
        private readonly IWeapon _weapon;
        private readonly WeaponAttachment _attachment;

        public WeaponDecorator(IWeapon weapon, WeaponAttachment attachment)
        {
            _weapon = weapon;
            _attachment = attachment;
        }

        public float Range => Mathf.Max(0f, _weapon.Range + _attachment.Range);
        public float Rate => Mathf.Max(0f, _weapon.Rate + _attachment.Rate);
        public float Strength => Mathf.Max(0f, _weapon.Strength + _attachment.Strength);
        public float Cooldown => Mathf.Max(0f, _weapon.Cooldown + _attachment.Cooldown);
    }
}
