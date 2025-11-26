using System.Collections.Generic;
using UnityEngine;

namespace BirdyFlap.UI
{
    /// <summary>
    /// ScriptableObject that manages UI navigation state.
    /// Acts as a central hub for panel state management.
    /// Create via Assets > Create > BirdyFlap > UI > Navigation Channel
    /// </summary>
    [CreateAssetMenu(fileName = "UINavigationChannel", menuName = "BirdyFlap/UI/Navigation Channel")]
    public class UINavigationChannel : ScriptableObject
    {
        [Header("Configuration")]
        [Tooltip("The panel to show when the game starts")]
        [SerializeField] private UIPanelState defaultPanel;
        
        [Tooltip("Enable debug logging")]
        [SerializeField] private bool debugMode;
        
        // Navigation stack for back navigation
        private readonly Stack<UIPanelState> navigationStack = new Stack<UIPanelState>();
        private UIPanelState currentPanel;
        
        /// <summary>
        /// Event raised when navigation occurs. Provides old and new panel states.
        /// </summary>
        public event System.Action<UIPanelState, UIPanelState> OnNavigate;
        
        /// <summary>
        /// Gets the currently active panel.
        /// </summary>
        public UIPanelState CurrentPanel => currentPanel;
        
        /// <summary>
        /// Gets the default panel for initial navigation.
        /// </summary>
        public UIPanelState DefaultPanel => defaultPanel;
        
        /// <summary>
        /// Returns true if there's a panel to navigate back to.
        /// </summary>
        public bool CanGoBack => navigationStack.Count > 0 || 
                                 (currentPanel != null && currentPanel.ParentPanel != null);
        
        /// <summary>
        /// Navigates to a specific panel, optionally adding current panel to history.
        /// </summary>
        /// <param name="panel">The panel to navigate to</param>
        /// <param name="addToHistory">If true, current panel is added to navigation stack</param>
        public void NavigateTo(UIPanelState panel, bool addToHistory = true)
        {
            if (panel == null)
            {
                LogDebug("Attempted to navigate to null panel");
                return;
            }
            
            if (currentPanel == panel)
            {
                LogDebug($"Already on panel: {panel.DisplayName}");
                return;
            }
            
            var previousPanel = currentPanel;
            
            // Add current to history if requested
            if (addToHistory && currentPanel != null)
            {
                navigationStack.Push(currentPanel);
            }
            
            // Update states
            if (previousPanel != null)
            {
                previousPanel.SetActive(false);
            }
            
            currentPanel = panel;
            currentPanel.SetActive(true);
            
            LogDebug($"Navigated from '{previousPanel?.DisplayName ?? "None"}' to '{panel.DisplayName}'");
            
            OnNavigate?.Invoke(previousPanel, currentPanel);
        }
        
        /// <summary>
        /// Navigates back to the previous panel in history.
        /// </summary>
        /// <returns>True if navigation occurred, false if no history</returns>
        public bool NavigateBack()
        {
            if (currentPanel == null || !currentPanel.AllowBackNavigation)
            {
                LogDebug("Back navigation not allowed for current panel");
                return false;
            }
            
            UIPanelState targetPanel = null;
            
            // First check navigation stack
            if (navigationStack.Count > 0)
            {
                targetPanel = navigationStack.Pop();
            }
            // Fall back to parent panel
            else if (currentPanel.ParentPanel != null)
            {
                targetPanel = currentPanel.ParentPanel;
            }
            
            if (targetPanel != null)
            {
                var previousPanel = currentPanel;
                previousPanel.SetActive(false);
                
                currentPanel = targetPanel;
                currentPanel.SetActive(true);
                
                LogDebug($"Navigated back to '{targetPanel.DisplayName}'");
                
                OnNavigate?.Invoke(previousPanel, currentPanel);
                return true;
            }
            
            LogDebug("No panel to navigate back to");
            return false;
        }
        
        /// <summary>
        /// Clears navigation history and navigates to the default panel.
        /// </summary>
        public void NavigateToDefault()
        {
            ClearHistory();
            
            if (defaultPanel != null)
            {
                NavigateTo(defaultPanel, false);
            }
        }
        
        /// <summary>
        /// Clears the navigation history stack.
        /// </summary>
        public void ClearHistory()
        {
            navigationStack.Clear();
            LogDebug("Navigation history cleared");
        }
        
        /// <summary>
        /// Resets the navigation channel state. Called when entering play mode.
        /// </summary>
        private void OnEnable()
        {
            navigationStack.Clear();
            currentPanel = null;
        }
        
        private void LogDebug(string message)
        {
#if UNITY_EDITOR
            if (debugMode)
            {
                Debug.Log($"[UINavigation] {message}", this);
            }
#endif
        }
    }
}
