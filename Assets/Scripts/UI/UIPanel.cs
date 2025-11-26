using UnityEngine;

namespace BirdyFlap.UI
{
    /// <summary>
    /// Base component for UI panels that work with the ScriptableObject navigation system.
    /// Attach to panel GameObjects and assign the corresponding UIPanelState.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class UIPanel : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("The panel state ScriptableObject for this panel")]
        [SerializeField] private UIPanelState panelState;
        
        [Tooltip("Should this panel start hidden?")]
        [SerializeField] private bool startHidden = true;
        
        [Header("Animation")]
        [Tooltip("Optional animator for show/hide transitions")]
        [SerializeField] private Animator animator;
        
        [Tooltip("Use CanvasGroup fade for transitions")]
        [SerializeField] private bool useFadeTransition = true;
        
        [SerializeField] private float fadeDuration = 0.2f;
        
        private CanvasGroup canvasGroup;
        private Coroutine fadeCoroutine;
        
        public UIPanelState PanelState => panelState;
        
        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            
            if (startHidden)
            {
                SetVisibility(false, false);
            }
        }
        
        private void OnEnable()
        {
            if (panelState != null)
            {
                panelState.OnActiveStateChanged += HandleActiveStateChanged;
                
                // Sync with current state
                SetVisibility(panelState.IsActive, false);
            }
        }
        
        private void OnDisable()
        {
            if (panelState != null)
            {
                panelState.OnActiveStateChanged -= HandleActiveStateChanged;
            }
        }
        
        private void HandleActiveStateChanged(bool isActive)
        {
            SetVisibility(isActive, true);
        }
        
        /// <summary>
        /// Sets the panel visibility with optional animation.
        /// </summary>
        public void SetVisibility(bool visible, bool animate = true)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            
            if (animate && useFadeTransition && fadeDuration > 0)
            {
                fadeCoroutine = StartCoroutine(FadeCoroutine(visible));
            }
            else
            {
                ApplyVisibility(visible);
            }
            
            // Trigger animator if present
            if (animator != null && animate)
            {
                string trigger = visible ? panelState.ShowAnimationTrigger : panelState.HideAnimationTrigger;
                if (!string.IsNullOrEmpty(trigger))
                {
                    animator.SetTrigger(trigger);
                }
            }
        }
        
        private System.Collections.IEnumerator FadeCoroutine(bool fadeIn)
        {
            float startAlpha = canvasGroup.alpha;
            float targetAlpha = fadeIn ? 1f : 0f;
            float elapsed = 0f;
            
            // Enable interaction at start if fading in
            if (fadeIn)
            {
                gameObject.SetActive(true);
            }
            
            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / fadeDuration;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                yield return null;
            }
            
            ApplyVisibility(fadeIn);
            fadeCoroutine = null;
        }
        
        private void ApplyVisibility(bool visible)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
            gameObject.SetActive(visible);
        }
        
        /// <summary>
        /// Shows this panel through the navigation system.
        /// </summary>
        public void Show()
        {
            if (panelState != null)
            {
                panelState.SetActive(true);
            }
        }
        
        /// <summary>
        /// Hides this panel through the navigation system.
        /// </summary>
        public void Hide()
        {
            if (panelState != null)
            {
                panelState.SetActive(false);
            }
        }
    }
}
