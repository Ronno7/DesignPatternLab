using DesignPatternLab.Characters.Drone;
using UnityEngine;
using UnityEngine.Pool;

namespace DesignPatternLab.Systems.Pooling
{
    [DisallowMultipleComponent]
    public class DroneObjectPool : MonoBehaviour
    {
        [Min(1)] public int maxPoolSize = 10;
        [Min(0)] public int stackDefaultCapacity = 10;
        [SerializeField] private Transform spawnCenter;
        [Min(0.1f)] public float droneLifetime = 3f;

        // The strategy client chooses behavior; the pool still owns reuse.
        public event System.Action<Drone> Spawned;

        private ObjectPool<Drone> _pool;

        public int ActiveCount => _pool == null ? 0 : _pool.CountActive;
        public int InactiveCount => _pool == null ? 0 : _pool.CountInactive;
        public int CreatedCount { get; private set; }
        public int DestroyedCount { get; private set; }

        public IObjectPool<Drone> Pool
        {
            get
            {
                if (_pool == null)
                {
                    _pool = new ObjectPool<Drone>(
                        CreatedPooledItem,
                        OnTakeFromPool,
                        OnReturnedToPool,
                        OnDestroyPoolObject,
                        true,
                        Mathf.Max(0, stackDefaultCapacity),
                        Mathf.Max(1, maxPoolSize));
                }

                return _pool;
            }
        }

        private Drone CreatedPooledItem()
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);

            // Assign ownership before Drone.OnEnable starts its lifetime.
            go.SetActive(false);
            go.transform.SetParent(transform, false);
            go.name = "Drone " + (++CreatedCount);

            Drone drone = go.AddComponent<Drone>();
            drone.Pool = Pool;
            return drone;
        }

        private void OnTakeFromPool(Drone drone)
        {
            // Place drones above the road and ahead of the follow camera.
            Vector3 center = spawnCenter ? spawnCenter.position : transform.position;
            Vector3 offset = Random.insideUnitSphere * 4f;
            offset.y = Random.Range(2f, 3f);
            offset.z += 8f;
            drone.transform.position = center + offset;
            Vector3 position = drone.transform.position;
            position.x = Mathf.Clamp(position.x, -3.5f, 3.5f);
            drone.transform.position = position;
            drone.transform.rotation = Quaternion.identity;
            drone.Lifetime = droneLifetime;
            drone.gameObject.SetActive(true);
            Spawned?.Invoke(drone);
        }

        private void OnReturnedToPool(Drone drone)
        {
            drone.gameObject.SetActive(false);
        }

        private void OnDestroyPoolObject(Drone drone)
        {
            DestroyedCount++;

            if (drone)
                Destroy(drone.gameObject);
        }

        public void Spawn()
        {
            if (!isActiveAndEnabled)
                return;

            // Random.Range with integer arguments excludes the upper bound.
            int amount = Random.Range(1, 10);

            for (int i = 0; i < amount; i++)
                Pool.Get();
        }

        private void OnDestroy()
        {
            if (_pool != null)
                _pool.Clear();

            // Active drones are children and leave with this scene object.
        }
    }
}
