using DesignPatternLab.Characters.Bike;
using DesignPatternLab.Systems.Replay;
using DesignPatternLab.Systems.Replay.Commands;
using UnityEngine;

namespace DesignPatternLab.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BikeController), typeof(Invoker))]
    public class ClientObserver : MonoBehaviour
    {
        private BikeController _bikeController;
        private Invoker _invoker;
        private Command _damageBike;

        private void Awake()
        {
            _bikeController = GetComponent<BikeController>();
            _invoker = GetComponent<Invoker>();
            _damageBike = new DamageBike(_bikeController, 15f);
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(Screen.width - 236, 476, 224, 72), GUI.skin.box);
            GUILayout.Label("Observer test - Chapter 9");

            bool wasEnabled = GUI.enabled;
            GUI.enabled = wasEnabled && _invoker.IsRecording && _bikeController.CurrentHealth > 0f;

            if (GUILayout.Button("Damage Bike (-15)"))
                _invoker.ExecuteCommand(_damageBike);

            GUI.enabled = wasEnabled;
            GUILayout.EndArea();
        }
    }
}
