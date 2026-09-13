using UnityEngine;

namespace DesignPatternLab.Characters.Bike
{
    // Adapted from David Baron, Chapter 5.
    public class BikeStartState : MonoBehaviour, IBikeState
    {
        private BikeController _bikeController;

        public void Handle(BikeController bikeController)
        {
            if (!_bikeController)
                _bikeController = bikeController;

            _bikeController.CurrentSpeed = _bikeController.maxSpeed;
        }

        private void Update()
        {
            // Baron's Turn action keeps moving forward until Stop sets speed to zero.
            if (_bikeController && _bikeController.CurrentSpeed > 0)
            {
                _bikeController.transform.Translate(
                    Vector3.forward * (_bikeController.CurrentSpeed * Time.deltaTime));
            }
        }
    }
}
