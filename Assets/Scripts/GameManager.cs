using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager instance is null!");
            }
            return _instance;
        }
    }
    
    private const string GAME_SCENE = "SampleScene";
    private const string MAIN_MENU_SCENE = "MainMenu";
    
    // Score tracking
    private int _currentScore;
    public int CurrentScore => _currentScore;
    
    public event System.Action<int> OnScoreChanged;
    
    private void Awake()
    {
        // Singleton pattern implementation
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void LoadGameScene()
    {
        ResetScore();
        SceneManager.LoadScene(GAME_SCENE);
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }

    public void RestartGameScene() {
        ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
    public void AddScore(int points)
    {
        _currentScore += points;
        OnScoreChanged?.Invoke(_currentScore);
    }
    
    public void ResetScore()
    {
        _currentScore = 0;
        OnScoreChanged?.Invoke(_currentScore);
    }
}

