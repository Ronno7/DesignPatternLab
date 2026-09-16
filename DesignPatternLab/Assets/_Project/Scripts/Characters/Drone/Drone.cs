using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace DesignPatternLab.Characters.Drone
{
    // Adapted from David Baron, Chapter 8.
    public class Drone : MonoBehaviour
    {
        public IObjectPool<Drone> Pool { get; set; }
        public float _currentHealth;

        [SerializeField, Min(1f)] private float maxHealth = 100f;
        [SerializeField, Min(0.1f)] private float timeToSelfDestruct = 3f;

        private bool _isReturned = true;

        private void OnEnable()
        {
            _isReturned = false;
            ResetDrone();
            AttackPlayer();
            StartCoroutine(SelfDestruct());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
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
            Pool.Release(this);
        }

        private void ResetDrone()
        {
            _currentHealth = maxHealth;
        }

        public void AttackPlayer()
        {
            // The chapter leaves actual attack behavior for a later lesson.
            Debug.Log("Attack player!", this);
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
