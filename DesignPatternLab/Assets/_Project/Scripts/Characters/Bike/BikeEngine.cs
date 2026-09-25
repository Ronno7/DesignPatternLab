using DesignPatternLab.Systems.PowerUps;
using UnityEngine;

namespace DesignPatternLab.Characters.Bike
{
    [DisallowMultipleComponent]
    public class BikeEngine : MonoBehaviour, IBikeElement
    {
        [Min(0f)] public float turboBoost = 25f;
        [Min(0f)] public float maxTurboBoost = 200f;

        // Scale the book's 300 mph baseline to this project's small demo track.
        // BikeController remains the single owner of the Turbo toggle.
        public float SpeedMultiplier(bool isTurboOn) => isTurboOn ? 1f + turboBoost / 300f : 1f;

        public void Accept(IVisitor visitor) => visitor.Visit(this);
    }
}
