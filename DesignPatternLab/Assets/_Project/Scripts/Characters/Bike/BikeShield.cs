using DesignPatternLab.Systems.PowerUps;
using UnityEngine;

namespace DesignPatternLab.Characters.Bike
{
    [DisallowMultipleComponent]
    public class BikeShield : MonoBehaviour, IBikeElement
    {
        [Range(0f, 100f)] public float health = 50f;

        public float Damage(float damage)
        {
            health = Mathf.Clamp(health - Mathf.Max(0f, damage), 0f, 100f);
            return health;
        }

        public void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
