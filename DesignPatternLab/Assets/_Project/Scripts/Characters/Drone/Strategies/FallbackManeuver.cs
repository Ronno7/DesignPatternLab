using System.Collections;
using UnityEngine;

namespace DesignPatternLab.Characters.Drone
{
    [DisallowMultipleComponent]
    public class FallbackManeuver : MonoBehaviour, IManeuverBehaviour
    {
        public void Maneuver(Drone drone) => drone.StartManeuver(Fallback(drone));

        private IEnumerator Fallback(Drone drone)
        {
            Vector3 start = drone.transform.position;
            // The laser faces -Z; retreating from the approaching bike is +Z.
            Vector3 end = start + Vector3.forward * Mathf.Max(0f, drone.fallbackDistance);
            float duration = Mathf.Max(0.01f, drone.speed);
            for (float time = 0f; time < duration; time += Time.deltaTime)
            {
                drone.transform.position = Vector3.Lerp(start, end, time / duration);
                yield return null;
            }

            drone.transform.position = end;
        }
    }
}
