using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    
    private void Start()
    {
        // Ensure main menu is visible and options is hidden at start
        ShowMainMenu();
    }
    
    public void OnStartGame()
    {
        GameManager.Instance.LoadGameScene();
    }
    
    public void OnOptions()
    {
        ShowOptionsPanel();
    }
    
    public void OnQuit()
    {
        GameManager.Instance.QuitGame();
    }
    
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
    
    public void ShowOptionsPanel()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
}

