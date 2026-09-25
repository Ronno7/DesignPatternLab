using DesignPatternLab.Systems.Pooling;
using UnityEngine;

namespace DesignPatternLab.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(DroneObjectPool))]
    public class ClientObjectPool : MonoBehaviour
    {
        private DroneObjectPool _pool;

        private void Awake()
        {
            _pool = GetComponent<DroneObjectPool>();
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(Screen.width - 236, 162, 224, 174), GUI.skin.box);
            GUILayout.Label("Drone pool - Chapter 8");

            if (GUILayout.Button("Spawn Drones"))
                _pool.Spawn();

            GUILayout.Label("Active: " + _pool.ActiveCount + "   In pool: " + _pool.InactiveCount);
            GUILayout.Label("Created: " + _pool.CreatedCount);
            GUILayout.Label("Destroyed: " + _pool.DestroyedCount);
            GUILayout.Label("Drones return after " + _pool.droneLifetime.ToString("0.#") + " seconds.");
            GUILayout.EndArea();
        }
    }
}
