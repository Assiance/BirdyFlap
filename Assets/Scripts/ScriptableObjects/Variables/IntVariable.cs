using UnityEngine;

namespace BirdyFlap.Variables
{
    /// <summary>
    /// ScriptableObject that holds an integer value.
    /// Enables data sharing between systems without direct references.
    /// Create via Assets > Create > BirdyFlap > Variables > Int Variable
    /// </summary>
    [CreateAssetMenu(fileName = "New Int Variable", menuName = "BirdyFlap/Variables/Int Variable")]
    public class IntVariable : ScriptableObject
    {
        [Tooltip("Description of what this variable represents")]
        [TextArea(2, 4)]
        [SerializeField] private string description;
        
        [Tooltip("The initial value when the game starts")]
        [SerializeField] private int initialValue;
        
        [Tooltip("Current runtime value")]
        [SerializeField] private int runtimeValue;
        
        [Header("Constraints")]
        [Tooltip("Use constraints to limit the value range")]
        [SerializeField] private bool useConstraints;
        
        [SerializeField] private int minValue;
        [SerializeField] private int maxValue = 100;
        
        /// <summary>
        /// Event raised when the value changes. Provides old and new values.
        /// </summary>
        public event System.Action<int, int> OnValueChanged;
        
        /// <summary>
        /// Gets or sets the current value.
        /// </summary>
        public int Value
        {
            get => runtimeValue;
            set
            {
                int newValue = useConstraints ? Mathf.Clamp(value, minValue, maxValue) : value;
                
                if (runtimeValue != newValue)
                {
                    int oldValue = runtimeValue;
                    runtimeValue = newValue;
                    OnValueChanged?.Invoke(oldValue, newValue);
                }
            }
        }
        
        /// <summary>
        /// Gets the initial value.
        /// </summary>
        public int InitialValue => initialValue;
        
        /// <summary>
        /// Resets the runtime value to the initial value.
        /// </summary>
        public void ResetToInitial()
        {
            Value = initialValue;
        }
        
        /// <summary>
        /// Adds to the current value.
        /// </summary>
        public void Add(int amount)
        {
            Value += amount;
        }
        
        /// <summary>
        /// Subtracts from the current value.
        /// </summary>
        public void Subtract(int amount)
        {
            Value -= amount;
        }
        
        /// <summary>
        /// Increments the value by 1.
        /// </summary>
        public void Increment()
        {
            Value++;
        }
        
        /// <summary>
        /// Decrements the value by 1.
        /// </summary>
        public void Decrement()
        {
            Value--;
        }
        
        private void OnEnable()
        {
            // Reset to initial value when entering play mode
            runtimeValue = initialValue;
        }
        
        /// <summary>
        /// Implicit conversion to int.
        /// </summary>
        public static implicit operator int(IntVariable variable)
        {
            return variable.Value;
        }
        
        public override string ToString()
        {
            return runtimeValue.ToString();
        }
    }
}
