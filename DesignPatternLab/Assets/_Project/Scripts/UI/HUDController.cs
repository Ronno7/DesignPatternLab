using DesignPatternLab.Systems.Events;
using UnityEngine;

namespace DesignPatternLab.UI
{
    public class HUDController : MonoBehaviour
    {
        private bool _isDisplayOn;

        private void OnEnable()
        {
            RaceEventBus.Subscribe(RaceEventType.START, DisplayHUD);
            RaceEventBus.Subscribe(RaceEventType.STOP, HideHUD);
        }

        private void OnDisable()
        {
            RaceEventBus.Unsubscribe(RaceEventType.START, DisplayHUD);
            RaceEventBus.Unsubscribe(RaceEventType.STOP, HideHUD);
            HideHUD();
        }

        private void DisplayHUD()
        {
            _isDisplayOn = true;
        }

        private void HideHUD()
        {
            _isDisplayOn = false;
        }

        private void OnGUI()
        {
            if (!_isDisplayOn)
                return;

            GUILayout.BeginArea(new Rect(12, 12, 228, 80), GUI.skin.box);
            GUILayout.Label("Race started - Chapter 6");

            if (GUILayout.Button("Stop Race"))
                RaceEventBus.Publish(RaceEventType.STOP);

            GUILayout.EndArea();
        }
    }
}
