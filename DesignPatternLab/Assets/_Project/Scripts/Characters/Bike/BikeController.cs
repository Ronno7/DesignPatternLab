using DesignPatternLab.Systems.Events;
using DesignPatternLab.Systems.PowerUps;
using UnityEngine;

namespace DesignPatternLab.Characters.Bike
{
    // State/Command receiver, Observer subject, and Chapter 10 visitable structure.
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BikeShield), typeof(BikeEngine), typeof(BikeWeapon))]
    public class BikeController : Subject, IBikeElement
    {
        [Min(0f)] public float maxSpeed = 2.0f;
        [Min(0f)] public float turnDistance = 2.0f;
        [SerializeField] private float health = 100f;

        public float CurrentSpeed
        {
            get => _baseSpeed * (Engine ? Engine.SpeedMultiplier(_isTurboOn) : 1f);
            set => _baseSpeed = value;
        }
        public BikeShield Shield { get; private set; }
        public BikeEngine Engine { get; private set; }
        public BikeWeapon Weapon { get; private set; }
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
        private IBikeElement[] _bikeElements;
        private float _baseSpeed;
        private float _initialShieldHealth, _initialTurboBoost, _initialWeaponStrength;
        private int _initialWeaponRange;

        private void Awake()
        {
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
            _initialHealth = health;
            Shield = GetComponent<BikeShield>();
            Engine = GetComponent<BikeEngine>();
            Weapon = GetComponent<BikeWeapon>();
            Weapon.InitializeWeapon();
            _bikeElements = new IBikeElement[] { Shield, Engine, Weapon };
            _initialShieldHealth = Shield.health;
            _initialTurboBoost = Engine.turboBoost;
            _initialWeaponRange = Weapon.range;
            _initialWeaponStrength = Weapon.strength;

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
            if (Weapon)
                Weapon.StopFiring();
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
            Weapon.StopFiring();
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

        public void Accept(IVisitor visitor)
        {
            if (visitor == null)
                return;

            foreach (IBikeElement element in _bikeElements)
                element.Accept(visitor);

            NotifyObservers();
        }

        public void ResetPosition()
        {
            transform.position = _initialPosition;
            transform.rotation = _initialRotation;
            health = _initialHealth;
            Shield.health = _initialShieldHealth;
            Engine.turboBoost = _initialTurboBoost;
            Weapon.range = _initialWeaponRange;
            Weapon.strength = _initialWeaponStrength;
            Weapon.ResetForRun();
            StopBike();
        }
    }
}
