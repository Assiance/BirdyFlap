using UnityEngine;
using TMPro;
using BirdyFlap.Events;
using BirdyFlap.Variables;

namespace BirdyFlap.UI
{
    /// <summary>
    /// Game HUD controller that displays score and responds to game state changes.
    /// Uses ScriptableObject variables and events for decoupled data binding.
    /// </summary>
    public class GameHUDController : MonoBehaviour
    {
        [Header("Score Display")]
        [Tooltip("Current score variable")]
        [SerializeField] private IntVariable score;
        
        [Tooltip("High score variable")]
        [SerializeField] private IntVariable highScore;
        
        [Tooltip("Text element for current score")]
        [SerializeField] private TextMeshProUGUI scoreText;
        
        [Tooltip("Text element for high score")]
        [SerializeField] private TextMeshProUGUI highScoreText;
        
        [Tooltip("Format string for score display (use {0} for score)")]
        [SerializeField] private string scoreFormat = "{0}";
        
        [Tooltip("Format string for high score display")]
        [SerializeField] private string highScoreFormat = "Best: {0}";
        
        [Header("Game Over Panel")]
        [Tooltip("Game over panel state")]
        [SerializeField] private UIPanelState gameOverPanel;
        
        [Tooltip("Navigation channel")]
        [SerializeField] private UINavigationChannel navigationChannel;
        
        [Header("Events")]
        [Tooltip("Event raised when game is over")]
        [SerializeField] private GameEvent onGameOver;
        
        private void OnEnable()
        {
            // Subscribe to score changes
            if (score != null)
            {
                score.OnValueChanged += OnScoreChanged;
                UpdateScoreDisplay(score.Value);
            }
            
            if (highScore != null)
            {
                highScore.OnValueChanged += OnHighScoreChanged;
                UpdateHighScoreDisplay(highScore.Value);
            }
            
            // Subscribe to game over event
            if (onGameOver != null)
            {
                onGameOver.Subscribe(HandleGameOver);
            }
        }
        
        private void OnDisable()
        {
            if (score != null)
            {
                score.OnValueChanged -= OnScoreChanged;
            }
            
            if (highScore != null)
            {
                highScore.OnValueChanged -= OnHighScoreChanged;
            }
            
            if (onGameOver != null)
            {
                onGameOver.Unsubscribe(HandleGameOver);
            }
        }
        
        private void OnScoreChanged(int oldValue, int newValue)
        {
            UpdateScoreDisplay(newValue);
        }
        
        private void OnHighScoreChanged(int oldValue, int newValue)
        {
            UpdateHighScoreDisplay(newValue);
        }
        
        private void UpdateScoreDisplay(int value)
        {
            if (scoreText != null)
            {
                scoreText.text = string.Format(scoreFormat, value);
            }
        }
        
        private void UpdateHighScoreDisplay(int value)
        {
            if (highScoreText != null)
            {
                highScoreText.text = string.Format(highScoreFormat, value);
            }
        }
        
        private void HandleGameOver()
        {
            // Show game over panel
            if (navigationChannel != null && gameOverPanel != null)
            {
                navigationChannel.NavigateTo(gameOverPanel, false);
            }
        }
    }
}
