using UnityEngine;

namespace DesignPatternLab.Systems.Weapons
{
    // Plain C# component in the pattern, initialized through its constructor.
    public class Weapon : IWeapon
    {
        public float Range { get; }
        public float Rate { get; }
        public float Strength { get; }
        public float Cooldown { get; }

        public Weapon(WeaponConfig config)
            : this(config.Range, config.Rate, config.Strength, config.Cooldown) { }

        // Snapshot this bike's Visitor-upgraded values without changing a shared asset.
        public Weapon(float range, float rate, float strength, float cooldown)
        {
            Range = Mathf.Max(0f, range);
            Rate = Mathf.Max(0f, rate);
            Strength = Mathf.Max(0f, strength);
            Cooldown = Mathf.Max(0f, cooldown);
        }
    }
}
