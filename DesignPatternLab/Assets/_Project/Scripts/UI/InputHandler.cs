using DesignPatternLab.Characters.Bike;
using DesignPatternLab.Systems.Events;
using DesignPatternLab.Systems.Replay;
using DesignPatternLab.Systems.Replay.Commands;
using UnityEngine;

namespace DesignPatternLab.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BikeController), typeof(Invoker))]
    public class InputHandler : MonoBehaviour
    {
        private BikeController _bikeController;
        private Invoker _invoker;
        private Command _buttonA, _buttonD, _buttonW;
        private bool _isCountingDown;

        private void Awake()
        {
            _bikeController = GetComponent<BikeController>();
            _invoker = GetComponent<Invoker>();
            _buttonA = new TurnLeft(_bikeController);
            _buttonD = new TurnRight(_bikeController);
            _buttonW = new ToggleTurbo(_bikeController);
        }

        private void OnEnable()
        {
            RaceEventBus.Subscribe(RaceEventType.COUNTDOWN, PrepareRecording);
            RaceEventBus.Subscribe(RaceEventType.START, StartRecording);
            RaceEventBus.Subscribe(RaceEventType.STOP, StopRecording);
        }

        private void OnDisable()
        {
            RaceEventBus.Unsubscribe(RaceEventType.COUNTDOWN, PrepareRecording);
            RaceEventBus.Unsubscribe(RaceEventType.START, StartRecording);
            RaceEventBus.Unsubscribe(RaceEventType.STOP, StopRecording);
            StopRecording();
        }

        private void PrepareRecording()
        {
            _invoker.Stop();
            _bikeController.ResetPosition();
            _isCountingDown = true;
        }

        private void StartRecording()
        {
            _isCountingDown = false;

            // A replay also publishes START for the bike and HUD.
            // It must not erase the recording it is about to play.
            if (!_invoker.IsReplaying)
                _invoker.Record();
        }

        private void StopRecording()
        {
            _isCountingDown = false;
            _invoker.Stop();
        }

        private void StartReplay()
        {
            if (_isCountingDown || _invoker.IsRecording ||
                _invoker.IsReplaying || !_invoker.HasRecording)
                return;

            _bikeController.ResetPosition();
            _invoker.Replay();
            RaceEventBus.Publish(RaceEventType.START);
        }

        private void Update()
        {
            if (!_invoker.IsRecording)
                return;

            // The book maps commands to key releases.
            if (Input.GetKeyUp(KeyCode.A))
                _invoker.ExecuteCommand(_buttonA);

            if (Input.GetKeyUp(KeyCode.D))
                _invoker.ExecuteCommand(_buttonD);

            if (Input.GetKeyUp(KeyCode.W))
                _invoker.ExecuteCommand(_buttonW);
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(12, 212, 228, 234), GUI.skin.box);
            GUILayout.Label("Commands / Replay - Chapter 7");

            string status = _invoker.IsRecording ? "Recording" :
                _invoker.IsReplaying ? "Replaying" :
                _isCountingDown ? "Waiting for race start" :
                _invoker.HasRecording ? "Recording ready" : "No recording yet";
            GUILayout.Label(status);
            float time = _invoker.IsReplaying ? _invoker.ReplayTime : _invoker.RecordingTime;
            GUILayout.Label("Time: " + time.ToString("0.00") + " s");
            GUILayout.Label("Commands: " + _invoker.RecordedCommandCount);

            bool wasEnabled = GUI.enabled;
            GUI.enabled = wasEnabled && _invoker.IsRecording;

            if (GUILayout.Button("Turn Left (A)"))
                _invoker.ExecuteCommand(_buttonA);

            if (GUILayout.Button("Turn Right (D)"))
                _invoker.ExecuteCommand(_buttonD);

            if (GUILayout.Button("Toggle Turbo (W)"))
                _invoker.ExecuteCommand(_buttonW);

            GUI.enabled = wasEnabled && !_isCountingDown && _invoker.HasRecording &&
                !_invoker.IsRecording && !_invoker.IsReplaying;

            if (GUILayout.Button("Start Replay"))
                StartReplay();

            GUI.enabled = wasEnabled;
            GUILayout.Label("Keys act on release.");
            GUILayout.EndArea();
        }
    }
}
