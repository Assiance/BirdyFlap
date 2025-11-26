using UnityEngine;
using BirdyFlap.Config;
using BirdyFlap.Events;
using BirdyFlap.Variables;
using BirdyFlap.UI;

namespace BirdyFlap.Core
{
    /// <summary>
    /// Central container for all game runtime ScriptableObjects.
    /// Provides a single point of access to all shared game data.
    /// Create one instance and reference it wherever needed.
    /// </summary>
    [CreateAssetMenu(fileName = "GameRuntimeData", menuName = "BirdyFlap/Core/Game Runtime Data")]
    public class GameRuntimeData : ScriptableObject
    {
        [Header("Configuration")]
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private AudioSettings audioSettings;
        
        [Header("Scene References")]
        [SerializeField] private SceneReference mainMenuScene;
        [SerializeField] private SceneReference gameScene;
        
        [Header("Game State Variables")]
        [SerializeField] private IntVariable score;
        [SerializeField] private IntVariable highScore;
        [SerializeField] private BoolVariable isPaused;
        [SerializeField] private BoolVariable isGameOver;
        
        [Header("Game Events - Requests")]
        [SerializeField] private GameEvent onStartGameRequest;
        [SerializeField] private GameEvent onPauseRequest;
        [SerializeField] private GameEvent onResumeRequest;
        [SerializeField] private GameEvent onRestartRequest;
        [SerializeField] private GameEvent onQuitRequest;
        [SerializeField] private GameEvent onMainMenuRequest;
        
        [Header("Game Events - Notifications")]
        [SerializeField] private GameEvent onGameStarted;
        [SerializeField] private GameEvent onGamePaused;
        [SerializeField] private GameEvent onGameResumed;
        [SerializeField] private GameEvent onGameOver;
        [SerializeField] private GameEvent onPlayerDeath;
        
        [Header("UI")]
        [SerializeField] private UINavigationChannel navigationChannel;
        
        // Public accessors for configuration
        public GameSettings GameSettings => gameSettings;
        public AudioSettings AudioSettings => audioSettings;
        
        // Public accessors for scenes
        public SceneReference MainMenuScene => mainMenuScene;
        public SceneReference GameScene => gameScene;
        
        // Public accessors for state
        public IntVariable Score => score;
        public IntVariable HighScore => highScore;
        public BoolVariable IsPaused => isPaused;
        public BoolVariable IsGameOver => isGameOver;
        
        // Public accessors for request events
        public GameEvent OnStartGameRequest => onStartGameRequest;
        public GameEvent OnPauseRequest => onPauseRequest;
        public GameEvent OnResumeRequest => onResumeRequest;
        public GameEvent OnRestartRequest => onRestartRequest;
        public GameEvent OnQuitRequest => onQuitRequest;
        public GameEvent OnMainMenuRequest => onMainMenuRequest;
        
        // Public accessors for notification events
        public GameEvent OnGameStarted => onGameStarted;
        public GameEvent OnGamePaused => onGamePaused;
        public GameEvent OnGameResumed => onGameResumed;
        public GameEvent OnGameOver => onGameOver;
        public GameEvent OnPlayerDeath => onPlayerDeath;
        
        // UI
        public UINavigationChannel NavigationChannel => navigationChannel;
        
        /// <summary>
        /// Resets all runtime state to initial values.
        /// Call this when starting a new game.
        /// </summary>
        public void ResetGameState()
        {
            score?.ResetToInitial();
            isPaused?.SetFalse();
            isGameOver?.SetFalse();
        }
        
        /// <summary>
        /// Convenience method to start the game.
        /// </summary>
        public void StartGame()
        {
            onStartGameRequest?.Raise();
        }
        
        /// <summary>
        /// Convenience method to pause the game.
        /// </summary>
        public void PauseGame()
        {
            onPauseRequest?.Raise();
        }
        
        /// <summary>
        /// Convenience method to resume the game.
        /// </summary>
        public void ResumeGame()
        {
            onResumeRequest?.Raise();
        }
        
        /// <summary>
        /// Convenience method to quit the game.
        /// </summary>
        public void QuitGame()
        {
            onQuitRequest?.Raise();
        }
    }
}
