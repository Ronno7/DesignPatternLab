using DesignPatternLab.Systems.Events;
using UnityEngine;

namespace DesignPatternLab.UI
{
    // Chapter 6 test client. The demo components are attached in Main.
    public class ClientEventBus : MonoBehaviour
    {
        private bool _isButtonEnabled = true;

        private void OnEnable()
        {
            RaceEventBus.Subscribe(RaceEventType.STOP, Restart);
        }

        private void OnDisable()
        {
            RaceEventBus.Unsubscribe(RaceEventType.STOP, Restart);
        }

        private void Restart()
        {
            _isButtonEnabled = true;
        }

        private void OnGUI()
        {
            if (!_isButtonEnabled)
                return;

            GUILayout.BeginArea(new Rect(12, 12, 228, 80), GUI.skin.box);
            GUILayout.Label("Race ready - Chapter 6");

            if (GUILayout.Button("Start Countdown"))
            {
                _isButtonEnabled = false;
                RaceEventBus.Publish(RaceEventType.COUNTDOWN);
            }

            GUILayout.EndArea();
        }
    }
}
