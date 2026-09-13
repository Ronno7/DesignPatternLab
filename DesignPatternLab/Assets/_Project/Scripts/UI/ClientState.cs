using DesignPatternLab.Characters.Bike;
using UnityEngine;

namespace DesignPatternLab.UI
{
    // Baron's Chapter 5 test client, with a state display and a reset button.
    [RequireComponent(typeof(BikeController))]
    public class ClientState : MonoBehaviour
    {
        private BikeController _bikeController;
        private Vector3 _initialPosition;

        private void Awake()
        {
            _bikeController = GetComponent<BikeController>();
            _initialPosition = transform.position;
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(12, 12, 228, 286), GUI.skin.box);
            GUILayout.Label("Bike controls");
            GUILayout.Label("State: " + _bikeController.CurrentState.GetType().Name);
            GUILayout.Label("Speed: " + _bikeController.CurrentSpeed.ToString("0.0"));

            if (GUILayout.Button("Start Bike"))
                _bikeController.StartBike();

            if (GUILayout.Button("Turn Left"))
                _bikeController.Turn(Direction.Left);

            if (GUILayout.Button("Turn Right"))
                _bikeController.Turn(Direction.Right);

            if (GUILayout.Button("Stop Bike"))
                _bikeController.StopBike();

            if (GUILayout.Button("Reset Position"))
            {
                _bikeController.StopBike();
                transform.position = _initialPosition;
            }

            GUILayout.Label("Start before turning.");
            GUILayout.EndArea();
        }
    }
}
