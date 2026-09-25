using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace DesignPatternLab.Characters.Drone
{
    // Chapter 8 pooled object and Chapter 11 Strategy context.
    public class Drone : MonoBehaviour
    {
        public IObjectPool<Drone> Pool { get; set; }
        public float _currentHealth;

        [SerializeField, Min(1f)] private float maxHealth = 100f;
        [SerializeField, Min(0.1f)] private float timeToSelfDestruct = 3f;

        [Header("Chapter 11 maneuvers")]
        [Tooltip("Seconds per movement segment, as in the book's Lerp examples.")]
        [Min(0.01f)] public float speed = 1f;
        [Min(0f)] public float maxHeight = 5f;
        [Min(0f)] public float weavingDistance = 1.5f;
        [Min(0f)] public float fallbackDistance = 20f;
        [SerializeField, Min(0f)] private float rayDistance = 15f;

        public IManeuverBehaviour CurrentStrategy { get; private set; }
        public float Lifetime
        {
            get => timeToSelfDestruct;
            set => timeToSelfDestruct = Mathf.Max(0.1f, value);
        }
        public Vector3 LaserDirection => transform.TransformDirection(
            Quaternion.Euler(-45f, 0f, 0f) * Vector3.back);

        private bool _isReturned = true;
        private Coroutine _maneuver;

        private void OnEnable()
        {
            _isReturned = false;
            ResetDrone();
            StartCoroutine(SelfDestruct());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            _maneuver = null;
            CurrentStrategy = null;
            ResetDrone();
            _isReturned = true;
        }

        private IEnumerator SelfDestruct()
        {
            yield return new WaitForSeconds(timeToSelfDestruct);
            TakeDamage(maxHealth);
        }

        private void ReturnToPool()
        {
            if (_isReturned)
                return;

            _isReturned = true;
            if (Pool != null)
                Pool.Release(this);
            else
                gameObject.SetActive(false);
        }

        private void ResetDrone()
        {
            _currentHealth = maxHealth;
        }

        public void AttackPlayer()
        {
            Vector3 direction = LaserDirection;
            bool hit = Physics.Raycast(transform.position, direction, out RaycastHit hitInfo,
                rayDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
            Debug.DrawRay(transform.position, direction * (hit ? hitInfo.distance : rayDistance),
                hit ? Color.green : Color.blue);
        }

        private void Update()
        {
            // Match the chapter's diagnostic laser: no shield damage is applied.
            AttackPlayer();
        }

        public void ApplyStrategy(IManeuverBehaviour strategy)
        {
            if (!isActiveAndEnabled)
                return;

            StopManeuver();
            CurrentStrategy = strategy;
            strategy?.Maneuver(this);
        }

        // Own the coroutine here so pooling and strategy changes can cancel it.
        // Do not stop the separate Chapter 8 lifetime coroutine when switching.
        public void StartManeuver(IEnumerator maneuver)
        {
            StopManeuver();
            if (isActiveAndEnabled && maneuver != null)
                _maneuver = StartCoroutine(maneuver);
        }

        private void StopManeuver()
        {
            if (_maneuver != null)
                StopCoroutine(_maneuver);
            _maneuver = null;
        }

        public void TakeDamage(float amount)
        {
            if (_isReturned || !isActiveAndEnabled || amount <= 0f)
                return;

            _currentHealth -= amount;

            if (_currentHealth <= 0f)
                ReturnToPool();
        }
    }
}
