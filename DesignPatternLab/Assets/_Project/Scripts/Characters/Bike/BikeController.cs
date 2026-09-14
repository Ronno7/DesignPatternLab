using DesignPatternLab.Systems.Events;
using UnityEngine;

namespace DesignPatternLab.Characters.Bike
{
    // Adapted from David Baron, Chapter 5.
    [DisallowMultipleComponent]
    public class BikeController : MonoBehaviour
    {
        [Min(0f)] public float maxSpeed = 2.0f;
        [Min(0f)] public float turnDistance = 2.0f;

        public float CurrentSpeed { get; set; }
        public Direction CurrentTurnDirection { get; private set; }
        public IBikeState CurrentState => _bikeStateContext.CurrentState;

        private IBikeState _startState, _stopState, _turnState;
        private BikeStateContext _bikeStateContext;

        private void Awake()
        {
            // Initialize before other components can issue commands in Start.
            _bikeStateContext = new BikeStateContext(this);
            _startState = gameObject.AddComponent<BikeStartState>();
            _stopState = gameObject.AddComponent<BikeStopState>();
            _turnState = gameObject.AddComponent<BikeTurnState>();
            _bikeStateContext.Transition(_stopState);
        }

        private void OnEnable()
        {
            RaceEventBus.Subscribe(RaceEventType.START, StartBike);
            RaceEventBus.Subscribe(RaceEventType.STOP, StopBike);
        }

        private void OnDisable()
        {
            RaceEventBus.Unsubscribe(RaceEventType.START, StartBike);
            RaceEventBus.Unsubscribe(RaceEventType.STOP, StopBike);

            // The chapter's state components are separate MonoBehaviours.
            // Stop their movement when this controller is disabled.
            CurrentSpeed = 0;
        }

        public void StartBike()
        {
            _bikeStateContext.Transition(_startState);
        }

        public void StopBike()
        {
            _bikeStateContext.Transition(_stopState);
        }

        public void Turn(Direction direction)
        {
            CurrentTurnDirection = direction;
            _bikeStateContext.Transition(_turnState);
        }
    }
}
