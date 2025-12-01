using UnityEngine;
using UnityEngine.SceneManagement;
using BirdyFlap.Config;
using BirdyFlap.Events;
using BirdyFlap.Variables;

namespace BirdyFlap
{
    /// <summary>
    /// ScriptableObject-based Game Manager.
    /// Uses SO events for decoupled communication and SO configs for settings.
    /// This is a lightweight MonoBehaviour that connects SO events to game actions.
    /// </summary>
    public class GameManagerSO : MonoBehaviour
    {
        [Header("Scene References")]
        [Tooltip("Reference to the main game scene")]
        [SerializeField] private SceneReference gameScene;
        
        [Tooltip("Reference to the main menu scene")]
        [SerializeField] private SceneReference mainMenuScene;
        
        [Header("Game State Variables")]
        [Tooltip("Is the game currently paused?")]
        [SerializeField] private BoolVariable isPaused;
        
        [Tooltip("Is the game over?")]
        [SerializeField] private BoolVariable isGameOver;
        
        [Tooltip("Current score")]
        [SerializeField] private IntVariable score;
        
        [Tooltip("High score")]
        [SerializeField] private IntVariable highScore;
        
        [Header("Game Events - Listen")]
        [Tooltip("Event raised when game should start")]
        [SerializeField] private GameEvent onStartGameRequest;
        
        [Tooltip("Event raised when game should pause")]
        [SerializeField] private GameEvent onPauseRequest;
        
        [Tooltip("Event raised when game should resume")]
        [SerializeField] private GameEvent onResumeRequest;
        
        [Tooltip("Event raised when game should restart")]
        [SerializeField] private GameEvent onRestartRequest;
        
        [Tooltip("Event raised when player wants to quit")]
        [SerializeField] private GameEvent onQuitRequest;
        
        [Tooltip("Event raised when returning to main menu")]
        [SerializeField] private GameEvent onMainMenuRequest;
        
        [Tooltip("Event raised when player dies")]
        [SerializeField] private GameEvent onPlayerDeath;
        
        [Header("Game Events - Raise")]
        [Tooltip("Event raised when game actually starts")]
        [SerializeField] private GameEvent onGameStarted;
        
        [Tooltip("Event raised when game is paused")]
        [SerializeField] private GameEvent onGamePaused;
        
        [Tooltip("Event raised when game is resumed")]
        [SerializeField] private GameEvent onGameResumed;
        
        [Tooltip("Event raised when game is over")]
        [SerializeField] private GameEvent onGameOver;
        
        [Header("Settings")]
        [Tooltip("Game configuration")]
        [SerializeField] private GameSettings gameSettings;
        
        private static GameManagerSO instance;
        
        /// <summary>
        /// Singleton instance accessor.
        /// </summary>
        public static GameManagerSO Instance => instance;
        
        /// <summary>
        /// Gets the current game settings.
        /// </summary>
        public GameSettings Settings => gameSettings;
        
        private void Awake()
        {
            // Singleton setup
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Load high score from PlayerPrefs
            LoadHighScore();
        }
        
        private void OnEnable()
        {
            // Subscribe to events
            SubscribeToEvent(onStartGameRequest, HandleStartGame);
            SubscribeToEvent(onPauseRequest, HandlePause);
            SubscribeToEvent(onResumeRequest, HandleResume);
            SubscribeToEvent(onRestartRequest, HandleRestart);
            SubscribeToEvent(onQuitRequest, HandleQuit);
            SubscribeToEvent(onMainMenuRequest, HandleMainMenu);
            SubscribeToEvent(onPlayerDeath, HandlePlayerDeath);
        }
        
        private void OnDisable()
        {
            // Unsubscribe from events (only if we're the active instance)
            if (instance == this)
            {
                UnsubscribeFromEvent(onStartGameRequest, HandleStartGame);
                UnsubscribeFromEvent(onPauseRequest, HandlePause);
                UnsubscribeFromEvent(onResumeRequest, HandleResume);
                UnsubscribeFromEvent(onRestartRequest, HandleRestart);
                UnsubscribeFromEvent(onQuitRequest, HandleQuit);
                UnsubscribeFromEvent(onMainMenuRequest, HandleMainMenu);
                UnsubscribeFromEvent(onPlayerDeath, HandlePlayerDeath);
            }
        }
        
        #region Event Handlers
        
        private void HandleStartGame()
        {
            // Reset game state
            if (score != null) score.ResetToInitial();
            if (isPaused != null) isPaused.SetFalse();
            if (isGameOver != null) isGameOver.SetFalse();
            
            // Ensure normal time scale
            Time.timeScale = 1f;
            
            // Load game scene
            if (gameScene != null && gameScene.IsValid)
            {
                SceneManager.LoadScene(gameScene.SceneName);
            }
            
            onGameStarted?.Raise();
        }
        
        private void HandlePause()
        {
            if (isPaused != null && !isPaused.Value)
            {
                isPaused.SetTrue();
                Time.timeScale = 0f;
                onGamePaused?.Raise();
            }
        }
        
        private void HandleResume()
        {
            if (isPaused != null && isPaused.Value)
            {
                isPaused.SetFalse();
                Time.timeScale = 1f;
                onGameResumed?.Raise();
            }
        }
        
        private void HandleRestart()
        {
            // Reset game state
            if (score != null) score.ResetToInitial();
            if (isPaused != null) isPaused.SetFalse();
            if (isGameOver != null) isGameOver.SetFalse();
            
            Time.timeScale = 1f;
            
            // Reload current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
            onGameStarted?.Raise();
        }
        
        private void HandleMainMenu()
        {
            Time.timeScale = 1f;
            
            if (mainMenuScene != null && mainMenuScene.IsValid)
            {
                SceneManager.LoadScene(mainMenuScene.SceneName);
            }
        }
        
        private void HandleQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        
        private void HandlePlayerDeath()
        {
            if (isGameOver != null)
            {
                isGameOver.SetTrue();
            }
            
            // Update high score
            UpdateHighScore();
            
            onGameOver?.Raise();
        }
        
        #endregion
        
        #region Score Management
        
        /// <summary>
        /// Adds points to the current score.
        /// </summary>
        public void AddScore(int points)
        {
            if (score != null)
            {
                score.Add(points);
            }
        }
        
        private void UpdateHighScore()
        {
            if (score != null && highScore != null && score.Value > highScore.Value)
            {
                highScore.Value = score.Value;
                SaveHighScore();
            }
        }
        
        private void LoadHighScore()
        {
            if (highScore != null)
            {
                highScore.Value = PlayerPrefs.GetInt("HighScore", 0);
            }
        }
        
        private void SaveHighScore()
        {
            if (highScore != null)
            {
                PlayerPrefs.SetInt("HighScore", highScore.Value);
                PlayerPrefs.Save();
            }
        }
        
        #endregion
        
        #region Event Subscription Helpers
        
        private void SubscribeToEvent(GameEvent gameEvent, System.Action handler)
        {
            if (gameEvent != null)
            {
                gameEvent.Subscribe(handler);
            }
        }
        
        private void UnsubscribeFromEvent(GameEvent gameEvent, System.Action handler)
        {
            if (gameEvent != null)
            {
                gameEvent.Unsubscribe(handler);
            }
        }
        
        #endregion
    }
}
