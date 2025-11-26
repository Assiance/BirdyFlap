using UnityEngine;
using BirdyFlap.Events;

namespace BirdyFlap.UI
{
    /// <summary>
    /// Reusable UI button actions component.
    /// Provides common button actions that can be wired up in the inspector.
    /// </summary>
    public class UIButtonActions : MonoBehaviour
    {
        [Header("Navigation")]
        [Tooltip("The navigation channel for panel navigation")]
        [SerializeField] private UINavigationChannel navigationChannel;
        
        [Header("Game Events")]
        [Tooltip("Event to raise when starting the game")]
        [SerializeField] private GameEvent onStartGameEvent;
        
        [Tooltip("Event to raise when quitting")]
        [SerializeField] private GameEvent onQuitEvent;
        
        [Tooltip("Event to raise when pausing")]
        [SerializeField] private GameEvent onPauseEvent;
        
        [Tooltip("Event to raise when resuming")]
        [SerializeField] private GameEvent onResumeEvent;
        
        [Tooltip("Event to raise when restarting")]
        [SerializeField] private GameEvent onRestartEvent;
        
        /// <summary>
        /// Navigates to a specific panel. Wire this to a button OnClick.
        /// </summary>
        public void NavigateToPanel(UIPanelState panel)
        {
            navigationChannel?.NavigateTo(panel);
        }
        
        /// <summary>
        /// Navigates back to the previous panel.
        /// </summary>
        public void NavigateBack()
        {
            navigationChannel?.NavigateBack();
        }
        
        /// <summary>
        /// Raises the start game event.
        /// </summary>
        public void StartGame()
        {
            onStartGameEvent?.Raise();
        }
        
        /// <summary>
        /// Raises the quit event.
        /// </summary>
        public void QuitGame()
        {
            onQuitEvent?.Raise();
        }
        
        /// <summary>
        /// Raises the pause event.
        /// </summary>
        public void PauseGame()
        {
            onPauseEvent?.Raise();
        }
        
        /// <summary>
        /// Raises the resume event.
        /// </summary>
        public void ResumeGame()
        {
            onResumeEvent?.Raise();
        }
        
        /// <summary>
        /// Raises the restart event.
        /// </summary>
        public void RestartGame()
        {
            onRestartEvent?.Raise();
        }
    }
}
