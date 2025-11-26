using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BirdyFlap.Config
{
    /// <summary>
    /// ScriptableObject that holds a reference to a scene.
    /// Provides type-safe scene loading without hardcoded strings.
    /// Create via Assets > Create > BirdyFlap > Config > Scene Reference
    /// </summary>
    [CreateAssetMenu(fileName = "New Scene Reference", menuName = "BirdyFlap/Config/Scene Reference")]
    public class SceneReference : ScriptableObject
    {
        [Tooltip("Description of this scene's purpose")]
        [TextArea(2, 4)]
        [SerializeField] private string description;
        
#if UNITY_EDITOR
        [Tooltip("Drag the scene asset here")]
        [SerializeField] private SceneAsset sceneAsset;
#endif
        
        [Tooltip("Scene name (auto-populated in editor)")]
        [SerializeField] private string sceneName;
        
        /// <summary>
        /// Gets the scene name for loading.
        /// </summary>
        public string SceneName => sceneName;
        
        /// <summary>
        /// Returns true if a valid scene is assigned.
        /// </summary>
        public bool IsValid => !string.IsNullOrEmpty(sceneName);
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (sceneAsset != null)
            {
                sceneName = sceneAsset.name;
            }
        }
#endif
    }
}
