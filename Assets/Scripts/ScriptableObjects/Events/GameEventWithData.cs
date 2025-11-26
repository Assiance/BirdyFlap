using System.Collections.Generic;
using UnityEngine;

namespace BirdyFlap.Events
{
    /// <summary>
    /// Generic ScriptableObject-based event that carries data of type T.
    /// Useful for events that need to pass information (e.g., score updates, damage events).
    /// </summary>
    public abstract class GameEventWithData<T> : ScriptableObject
    {
        [TextArea(3, 6)]
        [SerializeField] private string description;
        
        private readonly HashSet<IGameEventListener<T>> listeners = new HashSet<IGameEventListener<T>>();
        
#if UNITY_EDITOR
        [SerializeField] private bool debugMode;
        [SerializeField] private T debugValue;
        
        [ContextMenu("Raise with Debug Value")]
        private void RaiseDebug()
        {
            Raise(debugValue);
        }
#endif
        
        /// <summary>
        /// Raises the event with the specified data, notifying all registered listeners.
        /// </summary>
        public void Raise(T data)
        {
#if UNITY_EDITOR
            if (debugMode)
            {
                Debug.Log($"[GameEvent<{typeof(T).Name}>] '{name}' raised with data: {data}", this);
            }
#endif
            foreach (var listener in listeners)
            {
                listener.OnEventRaised(data);
            }
        }
        
        public void RegisterListener(IGameEventListener<T> listener)
        {
            listeners.Add(listener);
        }
        
        public void UnregisterListener(IGameEventListener<T> listener)
        {
            listeners.Remove(listener);
        }
    }
    
    /// <summary>
    /// Interface for listeners that receive typed event data.
    /// </summary>
    public interface IGameEventListener<T>
    {
        void OnEventRaised(T data);
    }
}
