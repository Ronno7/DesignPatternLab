using UnityEngine;

namespace DesignPatternLab.CameraSystem
{
    // Demo helper: keep the moving placeholder visible.
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0f, 8f, -12f);

        private void LateUpdate()
        {
            if (!target)
                return;

            transform.position = target.position + offset;
            transform.LookAt(target.position + Vector3.forward * 3f);
        }
    }
}
