using UnityEngine;

namespace BirdyFlap.Config
{
    /// <summary>
    /// Audio configuration ScriptableObject.
    /// Stores volume levels and audio preferences.
    /// Create via Assets > Create > BirdyFlap > Config > Audio Settings
    /// </summary>
    [CreateAssetMenu(fileName = "AudioSettings", menuName = "BirdyFlap/Config/Audio Settings")]
    public class AudioSettings : ScriptableObject
    {
        [Header("Volume Settings")]
        [Range(0f, 1f)]
        [SerializeField] private float masterVolume = 1f;
        
        [Range(0f, 1f)]
        [SerializeField] private float musicVolume = 0.8f;
        
        [Range(0f, 1f)]
        [SerializeField] private float sfxVolume = 1f;
        
        [Header("Audio Preferences")]
        [SerializeField] private bool muteWhenUnfocused = true;
        
        // Runtime values (not serialized)
        private float runtimeMasterVolume;
        private float runtimeMusicVolume;
        private float runtimeSfxVolume;
        
        public float MasterVolume
        {
            get => runtimeMasterVolume;
            set
            {
                runtimeMasterVolume = Mathf.Clamp01(value);
                OnVolumeChanged?.Invoke();
            }
        }
        
        public float MusicVolume
        {
            get => runtimeMusicVolume;
            set
            {
                runtimeMusicVolume = Mathf.Clamp01(value);
                OnVolumeChanged?.Invoke();
            }
        }
        
        public float SfxVolume
        {
            get => runtimeSfxVolume;
            set
            {
                runtimeSfxVolume = Mathf.Clamp01(value);
                OnVolumeChanged?.Invoke();
            }
        }
        
        public bool MuteWhenUnfocused => muteWhenUnfocused;
        
        /// <summary>
        /// Gets the effective music volume (master * music).
        /// </summary>
        public float EffectiveMusicVolume => runtimeMasterVolume * runtimeMusicVolume;
        
        /// <summary>
        /// Gets the effective SFX volume (master * sfx).
        /// </summary>
        public float EffectiveSfxVolume => runtimeMasterVolume * runtimeSfxVolume;
        
        /// <summary>
        /// Event invoked when any volume setting changes.
        /// </summary>
        public event System.Action OnVolumeChanged;
        
        private void OnEnable()
        {
            // Initialize runtime values from serialized defaults
            ResetToDefaults();
        }
        
        /// <summary>
        /// Resets runtime values to the serialized defaults.
        /// </summary>
        public void ResetToDefaults()
        {
            runtimeMasterVolume = masterVolume;
            runtimeMusicVolume = musicVolume;
            runtimeSfxVolume = sfxVolume;
            OnVolumeChanged?.Invoke();
        }
        
        /// <summary>
        /// Saves current settings to PlayerPrefs.
        /// </summary>
        public void SaveToPlayerPrefs()
        {
            PlayerPrefs.SetFloat("Audio_Master", runtimeMasterVolume);
            PlayerPrefs.SetFloat("Audio_Music", runtimeMusicVolume);
            PlayerPrefs.SetFloat("Audio_SFX", runtimeSfxVolume);
            PlayerPrefs.Save();
        }
        
        /// <summary>
        /// Loads settings from PlayerPrefs.
        /// </summary>
        public void LoadFromPlayerPrefs()
        {
            if (PlayerPrefs.HasKey("Audio_Master"))
            {
                runtimeMasterVolume = PlayerPrefs.GetFloat("Audio_Master", masterVolume);
                runtimeMusicVolume = PlayerPrefs.GetFloat("Audio_Music", musicVolume);
                runtimeSfxVolume = PlayerPrefs.GetFloat("Audio_SFX", sfxVolume);
                OnVolumeChanged?.Invoke();
            }
        }
    }
}
