using DesignPatternLab.Characters.Bike;
using DesignPatternLab.Systems.Events;
using DesignPatternLab.Systems.Replay;
using DesignPatternLab.Systems.Replay.Commands;
using UnityEngine;

namespace DesignPatternLab.Systems.PowerUps
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider))]
    public class Pickup : MonoBehaviour
    {
        public PowerUp powerup;
        public bool IsConsumed { get; private set; }
        private Collider _collider;
        private Renderer[] _renderers;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _renderers = GetComponentsInChildren<Renderer>();
        }

        private void OnEnable()
        {
            RaceEventBus.Subscribe(RaceEventType.COUNTDOWN, Restore);
            RaceEventBus.Subscribe(RaceEventType.START, Restore);
        }

        private void OnDisable()
        {
            RaceEventBus.Unsubscribe(RaceEventType.COUNTDOWN, Restore);
            RaceEventBus.Unsubscribe(RaceEventType.START, Restore);
        }

        private void OnTriggerEnter(Collider other)
        {
            // The existing Bike has its collider on the Body child.
            TryCollect(other.GetComponentInParent<BikeController>());
        }

        public bool TryCollect(BikeController bike)
        {
            if (!isActiveAndEnabled || IsConsumed || !powerup || !bike || !bike.IsEngineOn)
                return false;

            Invoker invoker = bike.GetComponent<Invoker>();
            // Replay uses the recorded command, never a second physics pickup.
            if (!invoker || !invoker.IsRecording || invoker.IsReplaying)
                return false;

            invoker.ExecuteCommand(new ApplyPowerUp(bike, powerup, this));
            return true;
        }

        public void Consume() => SetConsumed(true);

        private void Restore() => SetConsumed(false);

        private void SetConsumed(bool consumed)
        {
            IsConsumed = consumed;
            _collider.enabled = !consumed;
            foreach (Renderer item in _renderers)
                item.enabled = !consumed;
        }
    }
}
