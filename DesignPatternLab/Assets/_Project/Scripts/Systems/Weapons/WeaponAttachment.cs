using UnityEngine;

namespace DesignPatternLab.Systems.Weapons
{
    [CreateAssetMenu(fileName = "WeaponAttachment", menuName = "DesignPatternLab/Weapon/Attachment")]
    public class WeaponAttachment : ScriptableObject, IWeapon
    {
        [SerializeField, Range(0f, 50f)] private float rate;
        [SerializeField, Range(0f, 50f)] private float range;
        [SerializeField, Range(0f, 100f)] private float strength;
        [SerializeField, Range(-5f, 0f)] private float cooldown;
        public string attachmentName;
        public GameObject attachmentPrefab;
        [TextArea] public string attachmentDescription;

        public float Rate => rate;
        public float Range => range;
        public float Strength => strength;
        public float Cooldown => cooldown;
    }
}
