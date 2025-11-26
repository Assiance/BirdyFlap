using System;
using System.Collections.Generic;
using UnityEngine;

namespace BirdyFlap.Events
{
    /// <summary>
    /// ScriptableObject-based event for decoupled communication.
    /// Supports both MonoBehaviour listeners and direct C# action subscriptions.
    /// Create instances via Assets > Create > BirdyFlap > Events > Game Event
    /// </summary>
    [CreateAssetMenu(fileName = "New Game Event", menuName = "BirdyFlap/Events/Game Event")]
    public class GameEvent : ScriptableObject
    {
        [TextArea(3, 6)]
        [SerializeField] private string description;
        
        private readonly HashSet<GameEventListener> listeners = new HashSet<GameEventListener>();
        private event Action onRaised;
        
#if UNITY_EDITOR
        [SerializeField] private bool debugMode;
        
        [ContextMenu("Raise Event (Debug)")]
        private void RaiseDebug()
        {
            Raise();
        }
#endif
        
        /// <summary>
        /// Raises the event, notifying all registered listeners and action subscribers.
        /// </summary>
        public void Raise()
        {
#if UNITY_EDITOR
            if (debugMode)
            {
                Debug.Log($"[GameEvent] '{name}' raised with {listeners.Count} listeners", this);
            }
#endif
            // Invoke C# action subscribers
            onRaised?.Invoke();
            
            // Notify MonoBehaviour listeners
            foreach (var listener in listeners)
            {
                listener.OnEventRaised();
            }
        }
        
        /// <summary>
        /// Registers a MonoBehaviour listener to receive event notifications.
        /// </summary>
        public void RegisterListener(GameEventListener listener)
        {
            listeners.Add(listener);
        }
        
        /// <summary>
        /// Unregisters a MonoBehaviour listener from receiving event notifications.
        /// </summary>
        public void UnregisterListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }
        
        /// <summary>
        /// Subscribes a C# action to this event.
        /// </summary>
        public void Subscribe(Action action)
        {
            onRaised += action;
        }
        
        /// <summary>
        /// Unsubscribes a C# action from this event.
        /// </summary>
        public void Unsubscribe(Action action)
        {
            onRaised -= action;
        }
        
        /// <summary>
        /// Operator for subscribing actions (e.g., gameEvent += MyHandler).
        /// </summary>
        public static GameEvent operator +(GameEvent gameEvent, Action action)
        {
            gameEvent.Subscribe(action);
            return gameEvent;
        }
        
        /// <summary>
        /// Operator for unsubscribing actions (e.g., gameEvent -= MyHandler).
        /// </summary>
        public static GameEvent operator -(GameEvent gameEvent, Action action)
        {
            gameEvent.Unsubscribe(action);
            return gameEvent;
        }
        
        /// <summary>
        /// Gets the current listener count. Useful for debugging.
        /// </summary>
        public int ListenerCount => listeners.Count;
        
        /// <summary>
        /// Clears all C# action subscribers. Called when entering play mode.
        /// </summary>
        private void OnEnable()
        {
            onRaised = null;
        }
    }
}
