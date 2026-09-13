using UnityEngine;

namespace DesignPatternLab.Characters.Bike
{
    // Adapted from David Baron, Chapter 5.
    public class BikeStopState : MonoBehaviour, IBikeState
    {
        private BikeController _bikeController;

        public void Handle(BikeController bikeController)
        {
            if (!_bikeController)
                _bikeController = bikeController;

            _bikeController.CurrentSpeed = 0;
        }
    }
}
