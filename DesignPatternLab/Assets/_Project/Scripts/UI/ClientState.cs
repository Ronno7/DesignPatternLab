using DesignPatternLab.Characters.Bike;
using UnityEngine;

namespace DesignPatternLab.UI
{
    // Chapter 5's local turn controls. Chapter 6's events now start/stop the race.
    [RequireComponent(typeof(BikeController))]
    public class ClientState : MonoBehaviour
    {
        private BikeController _bikeController;

        private void Awake()
        {
            _bikeController = GetComponent<BikeController>();
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(12, 104, 228, 160), GUI.skin.box);
            GUILayout.Label("Bike state - Chapter 5");
            GUILayout.Label("State: " + _bikeController.CurrentState.GetType().Name);
            GUILayout.Label("Speed: " + _bikeController.CurrentSpeed.ToString("0.0"));

            if (GUILayout.Button("Turn Left"))
                _bikeController.Turn(Direction.Left);

            if (GUILayout.Button("Turn Right"))
                _bikeController.Turn(Direction.Right);

            GUILayout.Label("Turn after the countdown.");
            GUILayout.EndArea();
        }
    }
}
