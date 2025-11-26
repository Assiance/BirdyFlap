using UnityEngine;
using TMPro;
using BirdyFlap.Variables;

namespace BirdyFlap.UI
{
    /// <summary>
    /// Simple score display component that binds to an IntVariable.
    /// Demonstrates the observer pattern with ScriptableObject variables.
    /// </summary>
    public class ScoreDisplay : MonoBehaviour
    {
        [Header("Data Binding")]
        [Tooltip("The IntVariable to display")]
        [SerializeField] private IntVariable variable;
        
        [Header("Display")]
        [Tooltip("Text component to update")]
        [SerializeField] private TextMeshProUGUI displayText;
        
        [Tooltip("Format string (use {0} for the value)")]
        [SerializeField] private string format = "{0}";
        
        [Tooltip("Prefix text before the value")]
        [SerializeField] private string prefix = "";
        
        [Tooltip("Suffix text after the value")]
        [SerializeField] private string suffix = "";
        
        [Header("Animation")]
        [Tooltip("Animate value changes")]
        [SerializeField] private bool animateChanges = true;
        
        [Tooltip("Scale punch amount on value change")]
        [SerializeField] private float punchScale = 1.2f;
        
        [Tooltip("Duration of the punch animation")]
        [SerializeField] private float punchDuration = 0.15f;
        
        private Vector3 originalScale;
        private Coroutine punchCoroutine;
        
        private void Awake()
        {
            if (displayText != null)
            {
                originalScale = displayText.transform.localScale;
            }
        }
        
        private void OnEnable()
        {
            if (variable != null)
            {
                variable.OnValueChanged += OnValueChanged;
                UpdateDisplay(variable.Value);
            }
        }
        
        private void OnDisable()
        {
            if (variable != null)
            {
                variable.OnValueChanged -= OnValueChanged;
            }
        }
        
        private void OnValueChanged(int oldValue, int newValue)
        {
            UpdateDisplay(newValue);
            
            if (animateChanges && newValue > oldValue)
            {
                PlayPunchAnimation();
            }
        }
        
        private void UpdateDisplay(int value)
        {
            if (displayText != null)
            {
                string formattedValue = string.Format(format, value);
                displayText.text = $"{prefix}{formattedValue}{suffix}";
            }
        }
        
        private void PlayPunchAnimation()
        {
            if (displayText == null) return;
            
            if (punchCoroutine != null)
            {
                StopCoroutine(punchCoroutine);
            }
            
            punchCoroutine = StartCoroutine(PunchScaleCoroutine());
        }
        
        private System.Collections.IEnumerator PunchScaleCoroutine()
        {
            var textTransform = displayText.transform;
            float elapsed = 0f;
            float halfDuration = punchDuration * 0.5f;
            
            // Scale up
            while (elapsed < halfDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / halfDuration;
                textTransform.localScale = Vector3.Lerp(originalScale, originalScale * punchScale, t);
                yield return null;
            }
            
            // Scale down
            elapsed = 0f;
            while (elapsed < halfDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = elapsed / halfDuration;
                textTransform.localScale = Vector3.Lerp(originalScale * punchScale, originalScale, t);
                yield return null;
            }
            
            textTransform.localScale = originalScale;
            punchCoroutine = null;
        }
        
        /// <summary>
        /// Manually refresh the display from the current variable value.
        /// </summary>
        public void Refresh()
        {
            if (variable != null)
            {
                UpdateDisplay(variable.Value);
            }
        }
    }
}
