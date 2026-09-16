using DesignPatternLab.Characters.Bike;
using DesignPatternLab.Systems.Events;
using UnityEngine;

namespace DesignPatternLab.UI
{
    public class HUDController : Observer
    {
        [SerializeField] private Transform target;

        private BikeController _bikeController;
        private bool _isDisplayOn;
        private bool _isTurboOn;
        private float _currentHealth;

        private void OnEnable()
        {
            _bikeController = target ? target.GetComponent<BikeController>() : null;

            if (_bikeController)
            {
                _bikeController.Attach(this);
                Notify(_bikeController);
                _isDisplayOn = _bikeController.IsEngineOn;
            }

            RaceEventBus.Subscribe(RaceEventType.START, DisplayHUD);
            RaceEventBus.Subscribe(RaceEventType.STOP, HideHUD);
        }

        private void OnDisable()
        {
            if (_bikeController)
                _bikeController.Detach(this);

            RaceEventBus.Unsubscribe(RaceEventType.START, DisplayHUD);
            RaceEventBus.Unsubscribe(RaceEventType.STOP, HideHUD);
            HideHUD();
        }

        public override void Notify(Subject subject)
        {
            BikeController bike = subject as BikeController;

            if (bike)
            {
                _isTurboOn = bike.IsTurboOn;
                _currentHealth = bike.CurrentHealth;
            }
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
            if (_isDisplayOn)
            {
                GUILayout.BeginArea(new Rect(12, 12, 228, 80), GUI.skin.box);
                GUILayout.Label("Race started - Chapter 6");

                if (GUILayout.Button("Stop Race"))
                    RaceEventBus.Publish(RaceEventType.STOP);

                GUILayout.EndArea();
            }

            GUILayout.BeginArea(new Rect(Screen.width - 236, 348, 224, 116), GUI.skin.box);
            GUILayout.Label("Bike HUD - Chapter 9");
            GUILayout.Label("Health: " + _currentHealth.ToString("0"));

            if (_isTurboOn)
                GUILayout.Label("Turbo Activated!");

            if (_currentHealth <= 50f)
                GUILayout.Label("WARNING: Low Health");

            if (_currentHealth <= 0f)
                GUILayout.Label("New countdown restores health.");

            GUILayout.EndArea();
        }
    }
}
