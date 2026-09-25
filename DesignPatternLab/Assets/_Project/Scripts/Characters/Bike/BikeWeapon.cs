using DesignPatternLab.Systems.PowerUps;
using UnityEngine;

namespace DesignPatternLab.Characters.Bike
{
    [DisallowMultipleComponent]
    public class BikeWeapon : MonoBehaviour, IBikeElement
    {
        [Header("Range")]
        [Min(0)] public int range = 5;
        [Min(0)] public int maxRange = 25;
        [Header("Strength")]
        [Min(0f)] public float strength = 25f;
        [Min(0f)] public float maxStrength = 50f;

        // The chapter's skeleton weapon; combat belongs to later chapters.
        public void Fire() => Debug.Log("Weapon fired!");

        public void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
