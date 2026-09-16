using DesignPatternLab.Characters.Bike;
using DesignPatternLab.Systems.Events;
using UnityEngine;

namespace DesignPatternLab.CameraSystem
{
    // Chapter 9 camera observer, including the existing follow-camera behavior.
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class CameraController : Observer
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 8f, -12f);
        [SerializeField, Min(0f)] private float shakeMagnitude = 0.1f;

        private BikeController _bikeController;
        private bool _isTurboOn;

        private void OnEnable()
        {
            _bikeController = target ? target.GetComponent<BikeController>() : null;

            if (_bikeController)
            {
                _bikeController.Attach(this);
                Notify(_bikeController);
            }
        }

        private void OnDisable()
        {
            if (_bikeController)
                _bikeController.Detach(this);

            _isTurboOn = false;
            FollowTarget();
        }

        public override void Notify(Subject subject)
        {
            BikeController bike = subject as BikeController;

            if (bike)
                _isTurboOn = bike.IsTurboOn;
        }

        private void LateUpdate()
        {
            if (!target)
                return;

            // Rebuild the steady follow pose every frame so shake cannot drift.
            FollowTarget();

            if (_isTurboOn)
                transform.position += Random.insideUnitSphere * shakeMagnitude;
        }

        private void FollowTarget()
        {
            if (!target)
                return;

            transform.position = target.position + offset;
            transform.LookAt(target.position + Vector3.forward * 3f);
        }
    }
}
