using UnityEngine;

public class OptionsMenuUI : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    
    public void OnBack()
    {
        // Return to main menu panel
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }
}

