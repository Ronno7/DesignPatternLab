using UnityEngine;

namespace DesignPatternLab.Systems.Events
{
    public abstract class Observer : MonoBehaviour
    {
        public abstract void Notify(Subject subject);
    }
}
