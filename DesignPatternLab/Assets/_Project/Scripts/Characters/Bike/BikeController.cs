using DesignPatternLab.Systems.Events;
using UnityEngine;

namespace DesignPatternLab.Characters.Bike
{
    // State/Command receiver, race subscriber, and Chapter 9 observer subject.
    [DisallowMultipleComponent]
    public class BikeController : Subject
    {
        [Min(0f)] public float maxSpeed = 2.0f;
        [Min(0f)] public float turnDistance = 2.0f;
        [SerializeField] private float health = 100f;

        public float CurrentSpeed { get; set; }
        public Direction CurrentTurnDirection { get; private set; }
        public IBikeState CurrentState => _bikeStateContext.CurrentState;
        public bool IsTurboOn => _isTurboOn;
        public bool IsEngineOn => _isEngineOn;
        public float CurrentHealth => health;

        private IBikeState _startState, _stopState, _turnState;
        private BikeStateContext _bikeStateContext;
        private bool _isTurboOn;
        private bool _isEngineOn;
        private float _initialHealth;
        private Vector3 _initialPosition;
        private Quaternion _initialRotation;

        private void Awake()
        {
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
            _initialHealth = health;

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
            _isEngineOn = false;
            _isTurboOn = false;
            NotifyObservers();
        }

        public void StartBike()
        {
            if (health <= 0f)
                return;

            _isEngineOn = true;
            _bikeStateContext.Transition(_startState);
            NotifyObservers();
        }

        public void StopBike()
        {
            _isEngineOn = false;
            _isTurboOn = false;
            _bikeStateContext.Transition(_stopState);
            NotifyObservers();
        }

        public void Turn(Direction direction)
        {
            CurrentTurnDirection = direction;
            _bikeStateContext.Transition(_turnState);
        }

        public void ToggleTurbo()
        {
            if (!_isEngineOn)
                return;

            _isTurboOn = !_isTurboOn;
            Debug.Log("Turbo Active: " + _isTurboOn);
            NotifyObservers();
        }

        public void TakeDamage(float amount)
        {
            if (amount <= 0f || health <= 0f)
                return;

            health = Mathf.Max(0f, health - amount);
            _isTurboOn = false;
            NotifyObservers();

            // Keep the receiver available for the existing replay/reset system.
            if (health <= 0f)
                RaceEventBus.Publish(RaceEventType.STOP);
        }

        public void ResetPosition()
        {
            transform.position = _initialPosition;
            transform.rotation = _initialRotation;
            health = _initialHealth;
            StopBike();
        }
    }
}
