using System.Collections;
using UnityEngine;

namespace DesignPatternLab.Characters.Drone
{
    [DisallowMultipleComponent]
    public class WeavingManeuver : MonoBehaviour, IManeuverBehaviour
    {
        public void Maneuver(Drone drone) => drone.StartManeuver(Weave(drone));

        private IEnumerator Weave(Drone drone)
        {
            Vector3 left = drone.transform.position;
            Vector3 right = left;
            // This demo's straight track is centered at world X = 0.
            left.x = -Mathf.Abs(drone.weavingDistance);
            right.x = Mathf.Abs(drone.weavingDistance);
            float duration = Mathf.Max(0.01f, drone.speed);
            bool reverse = false;

            while (true)
            {
                Vector3 start = drone.transform.position;
                Vector3 end = reverse ? left : right;
                for (float time = 0f; time < duration; time += Time.deltaTime)
                {
                    drone.transform.position = Vector3.Lerp(start, end, time / duration);
                    yield return null;
                }

                drone.transform.position = end;
                yield return new WaitForSeconds(1f);
                reverse = !reverse;
            }
        }
    }
}
