using UnityEngine;
using UnityEngine.InputSystem;

namespace BirdyFlap.UI
{
    /// <summary>
    /// Controller that manages UI navigation using the UINavigationChannel.
    /// Handles input for back navigation and initial panel setup.
    /// </summary>
    public class UINavigationController : MonoBehaviour
    {
        [Header("Navigation")]
        [Tooltip("The navigation channel ScriptableObject")]
        [SerializeField] private UINavigationChannel navigationChannel;
        
        [Header("Input")]
        [Tooltip("Enable escape/back button to navigate back")]
        [SerializeField] private bool handleBackInput = true;
        
        [Tooltip("Input action for back navigation (optional, defaults to Escape key)")]
        [SerializeField] private InputActionReference backInputAction;
        
        [Header("Initialization")]
        [Tooltip("Navigate to default panel on start")]
        [SerializeField] private bool navigateToDefaultOnStart = true;
        
        private InputAction escapeAction;
        
        private void Awake()
        {
            // Create fallback escape action if none provided
            if (backInputAction == null && handleBackInput)
            {
                escapeAction = new InputAction("Back", binding: "<Keyboard>/escape");
            }
        }
        
        private void Start()
        {
            if (navigateToDefaultOnStart && navigationChannel != null)
            {
                navigationChannel.NavigateToDefault();
            }
        }
        
        private void OnEnable()
        {
            if (backInputAction != null)
            {
                backInputAction.action.performed += OnBackInput;
                backInputAction.action.Enable();
            }
            else if (escapeAction != null)
            {
                escapeAction.performed += OnBackInput;
                escapeAction.Enable();
            }
        }
        
        private void OnDisable()
        {
            if (backInputAction != null)
            {
                backInputAction.action.performed -= OnBackInput;
            }
            else if (escapeAction != null)
            {
                escapeAction.performed -= OnBackInput;
                escapeAction.Dispose();
            }
        }
        
        private void OnBackInput(InputAction.CallbackContext context)
        {
            if (handleBackInput && navigationChannel != null)
            {
                navigationChannel.NavigateBack();
            }
        }
        
        /// <summary>
        /// Navigates to a specific panel.
        /// </summary>
        public void NavigateTo(UIPanelState panel)
        {
            navigationChannel?.NavigateTo(panel);
        }
        
        /// <summary>
        /// Navigates back to the previous panel.
        /// </summary>
        public void NavigateBack()
        {
            navigationChannel?.NavigateBack();
        }
        
        /// <summary>
        /// Navigates to the default panel, clearing history.
        /// </summary>
        public void NavigateToDefault()
        {
            navigationChannel?.NavigateToDefault();
        }
    }
}
