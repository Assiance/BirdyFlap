using UnityEngine;
using BirdyFlap.Events;

namespace BirdyFlap.UI
{
    /// <summary>
    /// ScriptableObject representing a UI panel's state and configuration.
    /// Enables decoupled panel management without direct references.
    /// Create via Assets > Create > BirdyFlap > UI > Panel State
    /// </summary>
    [CreateAssetMenu(fileName = "New Panel State", menuName = "BirdyFlap/UI/Panel State")]
    public class UIPanelState : ScriptableObject
    {
        [Header("Panel Identity")]
        [Tooltip("Unique identifier for this panel")]
        [SerializeField] private string panelId;
        
        [Tooltip("Display name for debugging")]
        [SerializeField] private string displayName;
        
        [Header("Navigation")]
        [Tooltip("Panel to show when pressing back/escape (null for no back navigation)")]
        [SerializeField] private UIPanelState parentPanel;
        
        [Tooltip("Can this panel be closed with escape/back button?")]
        [SerializeField] private bool allowBackNavigation = true;
        
        [Header("Behavior")]
        [Tooltip("Should this panel pause the game when shown?")]
        [SerializeField] private bool pauseGameWhenActive = false;
        
        [Tooltip("Should this panel capture input exclusively?")]
        [SerializeField] private bool isModal = false;
        
        [Header("Animation")]
        [Tooltip("Animation trigger for showing this panel")]
        [SerializeField] private string showAnimationTrigger = "Show";
        
        [Tooltip("Animation trigger for hiding this panel")]
        [SerializeField] private string hideAnimationTrigger = "Hide";
        
        [Header("Events")]
        [Tooltip("Event raised when this panel is shown")]
        [SerializeField] private GameEvent onPanelShown;
        
        [Tooltip("Event raised when this panel is hidden")]
        [SerializeField] private GameEvent onPanelHidden;
        
        // Runtime state
        private bool isActive;
        
        // Properties
        public string PanelId => panelId;
        public string DisplayName => displayName;
        public UIPanelState ParentPanel => parentPanel;
        public bool AllowBackNavigation => allowBackNavigation;
        public bool PauseGameWhenActive => pauseGameWhenActive;
        public bool IsModal => isModal;
        public string ShowAnimationTrigger => showAnimationTrigger;
        public string HideAnimationTrigger => hideAnimationTrigger;
        public bool IsActive => isActive;
        
        /// <summary>
        /// Event invoked when the active state changes.
        /// </summary>
        public event System.Action<bool> OnActiveStateChanged;
        
        /// <summary>
        /// Sets this panel as active and raises the shown event.
        /// </summary>
        public void SetActive(bool active)
        {
            if (isActive == active) return;
            
            isActive = active;
            OnActiveStateChanged?.Invoke(active);
            
            if (active)
            {
                onPanelShown?.Raise();
            }
            else
            {
                onPanelHidden?.Raise();
            }
        }
        
        /// <summary>
        /// Resets runtime state. Called when entering play mode.
        /// </summary>
        private void OnEnable()
        {
            isActive = false;
        }
        
        private void OnValidate()
        {
            // Auto-generate panel ID if empty
            if (string.IsNullOrEmpty(panelId))
            {
                panelId = name.Replace(" ", "").Replace("Panel", "");
            }
            
            if (string.IsNullOrEmpty(displayName))
            {
                displayName = name;
            }
        }
    }
}
