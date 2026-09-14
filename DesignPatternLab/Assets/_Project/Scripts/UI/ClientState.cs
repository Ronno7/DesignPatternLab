using DesignPatternLab.Characters.Bike;
using UnityEngine;

namespace DesignPatternLab.UI
{
    // State display only. InputHandler now sends all player actions through Invoker.
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
            GUILayout.BeginArea(new Rect(12, 104, 228, 96), GUI.skin.box);
            GUILayout.Label("Bike state - Chapter 5");
            GUILayout.Label("State: " + _bikeController.CurrentState.GetType().Name);
            GUILayout.Label("Speed: " + _bikeController.CurrentSpeed.ToString("0.0"));

            GUILayout.Label("Turbo: " + (_bikeController.IsTurboOn ? "On" : "Off"));
            GUILayout.EndArea();
        }
    }
}
