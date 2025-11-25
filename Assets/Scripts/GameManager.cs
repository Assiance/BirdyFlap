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
        SceneManager.LoadScene(GAME_SCENE);
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}

