using DesignPatternLab.Characters.Bike;
using UnityEngine;

namespace DesignPatternLab.Systems.PowerUps
{
    // Shared authoring data, never the bike's mutable runtime state.
    [CreateAssetMenu(fileName = "PowerUp", menuName = "DesignPatternLab/PowerUp")]
    public class PowerUp : ScriptableObject, IVisitor
    {
        public string powerupName;
        public GameObject powerupPrefab;
        [TextArea] public string powerupDescription;
        [Tooltip("Fully heal the shield (separate from Chapter 9 bike health).")]
        public bool healShield;
        [Range(0f, 50f)] public float turboBoost;
        [Range(0, 25)] public int weaponRange;
        [Range(0f, 50f)] public float weaponStrength;

        public void Visit(BikeShield shield)
        {
            if (healShield)
                shield.health = 100f;
        }

        public void Visit(BikeEngine engine)
        {
            engine.turboBoost = Mathf.Clamp(engine.turboBoost + turboBoost,
                0f, engine.maxTurboBoost);
        }

        public void Visit(BikeWeapon weapon)
        {
            weapon.range = Mathf.Clamp(weapon.range + weaponRange, 0, weapon.maxRange);
            weapon.strength = Mathf.Clamp(weapon.strength +
                Mathf.Round(weapon.strength * weaponStrength / 100f), 0f, weapon.maxStrength);
        }
    }
}
