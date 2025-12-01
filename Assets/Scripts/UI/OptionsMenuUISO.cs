using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BirdyFlap.Config;

namespace BirdyFlap.UI
{
    /// <summary>
    /// Options Menu UI controller using ScriptableObject architecture.
    /// Binds UI elements to AudioSettings ScriptableObject.
    /// </summary>
    public class OptionsMenuUISO : MonoBehaviour
    {
        [Header("Navigation")]
        [Tooltip("Navigation channel for panel management")]
        [SerializeField] private UINavigationChannel navigationChannel;
        
        [Header("Audio Settings")]
        [Tooltip("Audio settings ScriptableObject")]
        [SerializeField] private AudioSettings audioSettings;
        
        [Header("UI Elements - Master Volume")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private TextMeshProUGUI masterVolumeLabel;
        
        [Header("UI Elements - Music Volume")]
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private TextMeshProUGUI musicVolumeLabel;
        
        [Header("UI Elements - SFX Volume")]
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private TextMeshProUGUI sfxVolumeLabel;
        
        private void OnEnable()
        {
            // Initialize UI from settings
            InitializeSliders();
            
            // Subscribe to settings changes
            if (audioSettings != null)
            {
                audioSettings.OnVolumeChanged += RefreshUI;
            }
        }
        
        private void OnDisable()
        {
            if (audioSettings != null)
            {
                audioSettings.OnVolumeChanged -= RefreshUI;
            }
        }
        
        private void InitializeSliders()
        {
            if (audioSettings == null) return;
            
            // Setup master volume slider
            if (masterVolumeSlider != null)
            {
                masterVolumeSlider.value = audioSettings.MasterVolume;
                masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            }
            
            // Setup music volume slider
            if (musicVolumeSlider != null)
            {
                musicVolumeSlider.value = audioSettings.MusicVolume;
                musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            }
            
            // Setup SFX volume slider
            if (sfxVolumeSlider != null)
            {
                sfxVolumeSlider.value = audioSettings.SfxVolume;
                sfxVolumeSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
            }
            
            RefreshUI();
        }
        
        private void RefreshUI()
        {
            if (audioSettings == null) return;
            
            // Update labels
            if (masterVolumeLabel != null)
            {
                masterVolumeLabel.text = $"{Mathf.RoundToInt(audioSettings.MasterVolume * 100)}%";
            }
            
            if (musicVolumeLabel != null)
            {
                musicVolumeLabel.text = $"{Mathf.RoundToInt(audioSettings.MusicVolume * 100)}%";
            }
            
            if (sfxVolumeLabel != null)
            {
                sfxVolumeLabel.text = $"{Mathf.RoundToInt(audioSettings.SfxVolume * 100)}%";
            }
        }
        
        private void OnMasterVolumeChanged(float value)
        {
            if (audioSettings != null)
            {
                audioSettings.MasterVolume = value;
            }
        }
        
        private void OnMusicVolumeChanged(float value)
        {
            if (audioSettings != null)
            {
                audioSettings.MusicVolume = value;
            }
        }
        
        private void OnSfxVolumeChanged(float value)
        {
            if (audioSettings != null)
            {
                audioSettings.SfxVolume = value;
            }
        }
        
        /// <summary>
        /// Called when the Back button is pressed.
        /// </summary>
        public void OnBack()
        {
            // Save settings when leaving options
            audioSettings?.SaveToPlayerPrefs();
            
            navigationChannel?.NavigateBack();
        }
        
        /// <summary>
        /// Called when the Reset to Defaults button is pressed.
        /// </summary>
        public void OnResetDefaults()
        {
            audioSettings?.ResetToDefaults();
            
            // Update sliders to reflect reset values
            if (audioSettings != null)
            {
                if (masterVolumeSlider != null)
                    masterVolumeSlider.value = audioSettings.MasterVolume;
                    
                if (musicVolumeSlider != null)
                    musicVolumeSlider.value = audioSettings.MusicVolume;
                    
                if (sfxVolumeSlider != null)
                    sfxVolumeSlider.value = audioSettings.SfxVolume;
            }
        }
    }
}
