using System.Collections.Generic;
using UnityEngine;

namespace DesignPatternLab.Systems.Events
{
    // Baron's Chapter 9 subject: knows observers, not their particular behavior.
    public abstract class Subject : MonoBehaviour
    {
        private readonly List<Observer> _observers = new List<Observer>();

        public void Attach(Observer observer)
        {
            if (observer && !_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void Detach(Observer observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObservers()
        {
            // A copy allows an observer to detach during a notification.
            foreach (Observer observer in _observers.ToArray())
            {
                if (observer && observer.isActiveAndEnabled && _observers.Contains(observer))
                    observer.Notify(this);
            }
        }
    }
}
