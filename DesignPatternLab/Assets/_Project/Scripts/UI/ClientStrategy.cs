using DesignPatternLab.Characters.Drone;
using DesignPatternLab.Systems.Pooling;
using UnityEngine;

namespace DesignPatternLab.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(DroneObjectPool))]
    public class ClientStrategy : MonoBehaviour
    {
        public enum ManeuverChoice { Random, Bobbing, Weaving, Fallback }

        public ManeuverChoice selection;
        public Drone LatestDrone { get; private set; }
        private DroneObjectPool _pool;
        private static readonly string[] Choices = { "Random", "Bob", "Weave", "Fall" };

        private void Awake() => _pool = GetComponent<DroneObjectPool>();
        private void OnEnable() => _pool.Spawned += ApplySelectedStrategy;
        private void OnDisable() => _pool.Spawned -= ApplySelectedStrategy;

        public Drone SpawnDrone() => _pool.Pool.Get();

        public void ApplySelectedStrategy(Drone drone)
        {
            if (!drone || !drone.isActiveAndEnabled)
                return;

            // Add each concrete strategy once, then reuse it across pool cycles.
            IManeuverBehaviour[] strategies =
            {
                GetOrAdd<BoppingManeuver>(drone),
                GetOrAdd<WeavingManeuver>(drone),
                GetOrAdd<FallbackManeuver>(drone)
            };
            int index = selection == ManeuverChoice.Random
                ? Random.Range(0, strategies.Length) : (int)selection - 1;
            drone.ApplyStrategy(strategies[Mathf.Clamp(index, 0, strategies.Length - 1)]);
            LatestDrone = drone;
        }

        private static T GetOrAdd<T>(Drone drone) where T : Component
        {
            T component = drone.GetComponent<T>();
            return component ? component : drone.gameObject.AddComponent<T>();
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(252, 292, 244, 180), GUI.skin.box);
            GUILayout.Label("Drone maneuvers - Chapter 11");
            selection = (ManeuverChoice)GUILayout.Toolbar((int)selection, Choices);

            if (GUILayout.Button("Spawn Drone"))
                SpawnDrone();

            bool wasEnabled = GUI.enabled;
            bool hasDrone = LatestDrone && LatestDrone.isActiveAndEnabled;
            GUI.enabled = wasEnabled && hasDrone;
            if (GUILayout.Button("Apply to latest drone"))
                ApplySelectedStrategy(LatestDrone);
            GUI.enabled = wasEnabled;

            string strategy = hasDrone && LatestDrone.CurrentStrategy != null
                ? LatestDrone.CurrentStrategy.GetType().Name.Replace("Maneuver", "").Replace("Bopping", "Bobbing")
                : "No active drone selected";
            GUILayout.Label(strategy);
            GUILayout.Label("Laser ray: Scene view + Gizmos");
            GUILayout.EndArea();
        }
    }
}
