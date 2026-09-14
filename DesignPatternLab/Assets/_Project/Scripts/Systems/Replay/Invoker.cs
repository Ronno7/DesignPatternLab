using System.Collections.Generic;
using DesignPatternLab.Systems.Events;
using DesignPatternLab.Systems.Replay.Commands;
using UnityEngine;

namespace DesignPatternLab.Systems.Replay
{
    // Chapter 7: execute commands now, record them, then replay them by time.
    [DisallowMultipleComponent]
    public class Invoker : MonoBehaviour
    {
        // Several inputs can arrive in one fixed step. Keep their order together.
        private readonly SortedList<float, List<Command>> _recordedCommands =
            new SortedList<float, List<Command>>();

        private float _recordingTime;
        private float _replayTime;
        private int _replayIndex;

        public bool IsRecording { get; private set; }
        public bool IsReplaying { get; private set; }
        public bool HasRecording { get; private set; }
        public int RecordedCommandCount { get; private set; }
        public float RecordingTime => _recordingTime;
        public float ReplayTime => _replayTime;

        public void ExecuteCommand(Command command)
        {
            if (IsReplaying)
                return;

            command.Execute();

            if (!IsRecording)
                return;

            List<Command> commands;
            if (!_recordedCommands.TryGetValue(_recordingTime, out commands))
            {
                commands = new List<Command>();
                _recordedCommands.Add(_recordingTime, commands);
            }

            commands.Add(command);
            RecordedCommandCount++;
        }

        public void Record()
        {
            IsReplaying = false;
            IsRecording = true;
            HasRecording = false;
            _recordingTime = 0;
            _replayTime = 0;
            _replayIndex = 0;
            RecordedCommandCount = 0;
            _recordedCommands.Clear();
        }

        public void Stop()
        {
            if (IsRecording)
                HasRecording = true;

            IsRecording = false;
            IsReplaying = false;
        }

        public void Replay()
        {
            if (!HasRecording || IsRecording || IsReplaying)
                return;

            _replayTime = 0;
            _replayIndex = 0;
            IsReplaying = true;
        }

        private void FixedUpdate()
        {
            if (IsRecording)
                _recordingTime += Time.fixedDeltaTime;

            if (!IsReplaying)
                return;

            _replayTime = Mathf.Min(_replayTime + Time.fixedDeltaTime, _recordingTime);

            // Run every command due by this step, including timestamp zero.
            while (_replayIndex < _recordedCommands.Count &&
                   _recordedCommands.Keys[_replayIndex] <= _replayTime)
            {
                foreach (Command command in _recordedCommands.Values[_replayIndex])
                    command.Execute();

                _replayIndex++;
            }

            // Preserve any driving time between the last input and Stop Race.
            if (_replayTime >= _recordingTime)
            {
                IsReplaying = false;
                RaceEventBus.Publish(RaceEventType.STOP);
            }
        }

        private void OnDisable()
        {
            Stop();
        }
    }
}
