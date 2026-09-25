using UnityEngine;

namespace DesignPatternLab.Systems.Weapons
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "DesignPatternLab/Weapon/Config")]
    public class WeaponConfig : ScriptableObject, IWeapon
    {
        [SerializeField, Range(0f, 60f)] private float rate = 2f;
        [SerializeField, Range(0f, 50f)] private float range = 5f;
        [SerializeField, Range(0f, 100f)] private float strength = 25f;
        [SerializeField, Range(0f, 5f)] private float cooldown = 2f;
        public string weaponName;
        public GameObject weaponPrefab;
        [TextArea] public string weaponDescription;

        public float Rate => rate;
        public float Range => range;
        public float Strength => strength;
        public float Cooldown => cooldown;
    }
}
