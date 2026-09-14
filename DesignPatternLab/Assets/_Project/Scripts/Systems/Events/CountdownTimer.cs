using System.Collections;
using UnityEngine;

namespace DesignPatternLab.Systems.Events
{
    // COUNTDOWN starts the timer; reaching zero publishes START.
    public class CountdownTimer : MonoBehaviour
    {
        private const float Duration = 3.0f;
        private float _currentTime;
        private Coroutine _countdown;

        private void OnEnable()
        {
            RaceEventBus.Subscribe(RaceEventType.COUNTDOWN, StartTimer);
            RaceEventBus.Subscribe(RaceEventType.STOP, CancelTimer);
        }

        private void OnDisable()
        {
            RaceEventBus.Unsubscribe(RaceEventType.COUNTDOWN, StartTimer);
            RaceEventBus.Unsubscribe(RaceEventType.STOP, CancelTimer);
            CancelTimer();
        }

        private void StartTimer()
        {
            // A repeated request replaces the timer instead of stacking timers.
            CancelTimer();
            _countdown = StartCoroutine(Countdown());
        }

        private void CancelTimer()
        {
            if (_countdown != null)
            {
                StopCoroutine(_countdown);
                _countdown = null;
            }

            _currentTime = 0;
        }

        private IEnumerator Countdown()
        {
            _currentTime = Duration;

            while (_currentTime > 0)
            {
                yield return new WaitForSeconds(1f);
                _currentTime--;
            }

            _countdown = null;
            RaceEventBus.Publish(RaceEventType.START);
        }

        private void OnGUI()
        {
            if (_countdown == null)
                return;

            GUILayout.BeginArea(new Rect(12, 12, 228, 80), GUI.skin.box);
            GUILayout.Label("Race countdown - Chapter 6");
            GUILayout.Label("COUNTDOWN: " + _currentTime.ToString("0"));
            GUILayout.EndArea();
        }
    }
}
