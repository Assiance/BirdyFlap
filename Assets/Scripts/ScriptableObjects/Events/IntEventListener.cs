using UnityEngine;
using UnityEngine.Events;

namespace BirdyFlap.Events
{
    /// <summary>
    /// MonoBehaviour that listens for an IntEvent and invokes a UnityEvent with the int value.
    /// </summary>
    public class IntEventListener : MonoBehaviour, IGameEventListener<int>
    {
        [Tooltip("The IntEvent to listen for")]
        [SerializeField] private IntEvent intEvent;
        
        [Tooltip("Response to invoke when the event is raised")]
        [SerializeField] private UnityEvent<int> response;
        
        private void OnEnable()
        {
            if (intEvent != null)
            {
                intEvent.RegisterListener(this);
            }
        }
        
        private void OnDisable()
        {
            if (intEvent != null)
            {
                intEvent.UnregisterListener(this);
            }
        }
        
        public void OnEventRaised(int data)
        {
            response?.Invoke(data);
        }
    }
}
