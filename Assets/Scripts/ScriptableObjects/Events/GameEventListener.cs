using UnityEngine;
using UnityEngine.Events;

namespace BirdyFlap.Events
{
    /// <summary>
    /// MonoBehaviour that listens for a GameEvent and invokes a UnityEvent response.
    /// Attach to any GameObject to respond to ScriptableObject events.
    /// </summary>
    public class GameEventListener : MonoBehaviour
    {
        [Tooltip("The GameEvent to listen for")]
        [SerializeField] private GameEvent gameEvent;
        
        [Tooltip("Response to invoke when the event is raised")]
        [SerializeField] private UnityEvent response;
        
        private void OnEnable()
        {
            if (gameEvent != null)
            {
                gameEvent.RegisterListener(this);
            }
        }
        
        private void OnDisable()
        {
            if (gameEvent != null)
            {
                gameEvent.UnregisterListener(this);
            }
        }
        
        /// <summary>
        /// Called by the GameEvent when it's raised.
        /// </summary>
        public void OnEventRaised()
        {
            response?.Invoke();
        }
        
        /// <summary>
        /// Allows changing the event at runtime if needed.
        /// </summary>
        public void SetEvent(GameEvent newEvent)
        {
            if (gameEvent != null && enabled)
            {
                gameEvent.UnregisterListener(this);
            }
            
            gameEvent = newEvent;
            
            if (gameEvent != null && enabled)
            {
                gameEvent.RegisterListener(this);
            }
        }
    }
}
