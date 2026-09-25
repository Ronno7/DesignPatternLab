using System.Collections;
using UnityEngine;

namespace DesignPatternLab.Characters.Drone
{
    // Keep the book's class spelling; the maneuver is called "bobbing" in the UI.
    [DisallowMultipleComponent]
    public class BoppingManeuver : MonoBehaviour, IManeuverBehaviour
    {
        public void Maneuver(Drone drone) => drone.StartManeuver(Bob(drone));

        private IEnumerator Bob(Drone drone)
        {
            Vector3 bottom = drone.transform.position;
            Vector3 top = bottom;
            top.y = Mathf.Max(bottom.y, drone.maxHeight);
            float duration = Mathf.Max(0.01f, drone.speed);
            bool reverse = false;

            while (true)
            {
                Vector3 start = drone.transform.position;
                Vector3 end = reverse ? bottom : top;
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
