using UnityEngine;
using BirdyFlap.Events;

namespace BirdyFlap.UI
{
    /// <summary>
    /// Main Menu UI controller using ScriptableObject architecture.
    /// Uses events for decoupled communication with the GameManager.
    /// </summary>
    public class MainMenuUISO : MonoBehaviour
    {
        [Header("Navigation")]
        [Tooltip("Navigation channel for panel management")]
        [SerializeField] private UINavigationChannel navigationChannel;
        
        [Header("Panel States")]
        [Tooltip("Main menu panel state")]
        [SerializeField] private UIPanelState mainMenuPanel;
        
        [Tooltip("Options panel state")]
        [SerializeField] private UIPanelState optionsPanel;
        
        [Header("Game Events")]
        [Tooltip("Event to raise when starting the game")]
        [SerializeField] private GameEvent onStartGameRequest;
        
        [Tooltip("Event to raise when quitting")]
        [SerializeField] private GameEvent onQuitRequest;
        
        private void Start()
        {
            // Navigate to main menu panel on start
            if (navigationChannel != null && mainMenuPanel != null)
            {
                navigationChannel.NavigateTo(mainMenuPanel, false);
            }
        }
        
        /// <summary>
        /// Called when the Start/Play button is pressed.
        /// </summary>
        public void OnStartGame()
        {
            onStartGameRequest?.Raise();
        }
        
        /// <summary>
        /// Called when the Options button is pressed.
        /// </summary>
        public void OnOptions()
        {
            if (navigationChannel != null && optionsPanel != null)
            {
                navigationChannel.NavigateTo(optionsPanel);
            }
        }
        
        /// <summary>
        /// Called when the Quit button is pressed.
        /// </summary>
        public void OnQuit()
        {
            onQuitRequest?.Raise();
        }
        
        /// <summary>
        /// Called when the Back button is pressed (from any sub-panel).
        /// </summary>
        public void OnBack()
        {
            navigationChannel?.NavigateBack();
        }
    }
}
