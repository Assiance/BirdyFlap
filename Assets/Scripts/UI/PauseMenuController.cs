using UnityEngine;
using UnityEngine.InputSystem;
using BirdyFlap.Events;
using BirdyFlap.Variables;

namespace BirdyFlap.UI
{
    /// <summary>
    /// Pause menu controller that handles pause input and displays pause UI.
    /// Uses ScriptableObject events and variables for decoupled game state management.
    /// </summary>
    public class PauseMenuController : MonoBehaviour
    {
        [Header("State")]
        [Tooltip("Is the game currently paused?")]
        [SerializeField] private BoolVariable isPaused;
        
        [Tooltip("Is the game over? (Disable pause when game is over)")]
        [SerializeField] private BoolVariable isGameOver;
        
        [Header("Navigation")]
        [Tooltip("Navigation channel")]
        [SerializeField] private UINavigationChannel navigationChannel;
        
        [Tooltip("Pause menu panel state")]
        [SerializeField] private UIPanelState pausePanel;
        
        [Header("Events")]
        [Tooltip("Event to raise when requesting pause")]
        [SerializeField] private GameEvent onPauseRequest;
        
        [Tooltip("Event to raise when requesting resume")]
        [SerializeField] private GameEvent onResumeRequest;
        
        [Tooltip("Event to raise when requesting restart")]
        [SerializeField] private GameEvent onRestartRequest;
        
        [Tooltip("Event to raise when requesting main menu")]
        [SerializeField] private GameEvent onMainMenuRequest;
        
        [Header("Input")]
        [Tooltip("Input action for pause toggle")]
        [SerializeField] private InputActionReference pauseInputAction;
        
        private InputAction escapeAction;
        
        private void Awake()
        {
            // Create fallback escape action if none provided
            if (pauseInputAction == null)
            {
                escapeAction = new InputAction("Pause", binding: "<Keyboard>/escape");
            }
        }
        
        private void OnEnable()
        {
            if (pauseInputAction != null)
            {
                pauseInputAction.action.performed += OnPauseInput;
                pauseInputAction.action.Enable();
            }
            else if (escapeAction != null)
            {
                escapeAction.performed += OnPauseInput;
                escapeAction.Enable();
            }
            
            // Subscribe to pause state changes
            if (isPaused != null)
            {
                isPaused.OnValueChanged += OnPausedStateChanged;
            }
        }
        
        private void OnDisable()
        {
            if (pauseInputAction != null)
            {
                pauseInputAction.action.performed -= OnPauseInput;
            }
            else if (escapeAction != null)
            {
                escapeAction.performed -= OnPauseInput;
                escapeAction.Dispose();
            }
            
            if (isPaused != null)
            {
                isPaused.OnValueChanged -= OnPausedStateChanged;
            }
        }
        
        private void OnPauseInput(InputAction.CallbackContext context)
        {
            // Don't allow pause toggle when game is over
            if (isGameOver != null && isGameOver.Value) return;
            
            TogglePause();
        }
        
        private void OnPausedStateChanged(bool paused)
        {
            if (paused)
            {
                ShowPauseMenu();
            }
            else
            {
                HidePauseMenu();
            }
        }
        
        /// <summary>
        /// Toggles the pause state.
        /// </summary>
        public void TogglePause()
        {
            if (isPaused == null) return;
            
            if (isPaused.Value)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        
        /// <summary>
        /// Pauses the game.
        /// </summary>
        public void Pause()
        {
            onPauseRequest?.Raise();
        }
        
        /// <summary>
        /// Resumes the game.
        /// </summary>
        public void Resume()
        {
            onResumeRequest?.Raise();
        }
        
        /// <summary>
        /// Restarts the game.
        /// </summary>
        public void Restart()
        {
            onRestartRequest?.Raise();
        }
        
        /// <summary>
        /// Returns to main menu.
        /// </summary>
        public void MainMenu()
        {
            onMainMenuRequest?.Raise();
        }
        
        private void ShowPauseMenu()
        {
            if (navigationChannel != null && pausePanel != null)
            {
                navigationChannel.NavigateTo(pausePanel, false);
            }
        }
        
        private void HidePauseMenu()
        {
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);
            }
        }
    }
}
