using UnityEngine;

namespace BirdyFlap.Variables
{
    /// <summary>
    /// ScriptableObject that holds a boolean value.
    /// Useful for game states like isPaused, isGameOver, etc.
    /// Create via Assets > Create > BirdyFlap > Variables > Bool Variable
    /// </summary>
    [CreateAssetMenu(fileName = "New Bool Variable", menuName = "BirdyFlap/Variables/Bool Variable")]
    public class BoolVariable : ScriptableObject
    {
        [Tooltip("Description of what this variable represents")]
        [TextArea(2, 4)]
        [SerializeField] private string description;
        
        [Tooltip("The initial value when the game starts")]
        [SerializeField] private bool initialValue;
        
        [Tooltip("Current runtime value")]
        [SerializeField] private bool runtimeValue;
        
        /// <summary>
        /// Event raised when the value changes.
        /// </summary>
        public event System.Action<bool> OnValueChanged;
        
        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        public bool Value
        {
            get => runtimeValue;
            set
            {
                if (runtimeValue != value)
                {
                    runtimeValue = value;
                    OnValueChanged?.Invoke(value);
                }
            }
        }
        
        /// <summary>
        /// Gets the initial value.
        /// </summary>
        public bool InitialValue => initialValue;
        
        /// <summary>
        /// Resets the runtime value to the initial value.
        /// </summary>
        public void ResetToInitial()
        {
            Value = initialValue;
        }
        
        /// <summary>
        /// Sets value to true.
        /// </summary>
        public void SetTrue()
        {
            Value = true;
        }
        
        /// <summary>
        /// Sets value to false.
        /// </summary>
        public void SetFalse()
        {
            Value = false;
        }
        
        /// <summary>
        /// Toggles the current value.
        /// </summary>
        public void Toggle()
        {
            Value = !runtimeValue;
        }
        
        private void OnEnable()
        {
            // Reset to initial value when entering play mode
            runtimeValue = initialValue;
        }
        
        /// <summary>
        /// Implicit conversion to bool.
        /// </summary>
        public static implicit operator bool(BoolVariable variable)
        {
            return variable.Value;
        }
        
        public override string ToString()
        {
            return runtimeValue.ToString();
        }
    }
}
