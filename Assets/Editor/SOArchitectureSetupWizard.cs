#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using BirdyFlap.Config;
using BirdyFlap.Events;
using BirdyFlap.Variables;
using BirdyFlap.UI;
using BirdyFlap.Core;

namespace BirdyFlap.Editor
{
    /// <summary>
    /// Editor wizard to create all ScriptableObject assets for the SO architecture.
    /// Access via Window > BirdyFlap > Setup SO Architecture
    /// </summary>
    public class SOArchitectureSetupWizard : EditorWindow
    {
        private const string BASE_PATH = "Assets/ScriptableObjects";
        
        private bool createEvents = true;
        private bool createVariables = true;
        private bool createConfig = true;
        private bool createUI = true;
        private bool createRuntimeData = true;
        
        [MenuItem("Window/BirdyFlap/Setup SO Architecture")]
        public static void ShowWindow()
        {
            var window = GetWindow<SOArchitectureSetupWizard>("SO Architecture Setup");
            window.minSize = new Vector2(400, 300);
            window.Show();
        }
        
        private void OnGUI()
        {
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("ScriptableObject Architecture Setup", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);
            
            EditorGUILayout.HelpBox(
                "This wizard will create all the ScriptableObject assets needed for the SO-based architecture. " +
                "Assets will be created in: " + BASE_PATH,
                MessageType.Info
            );
            
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Select what to create:", EditorStyles.boldLabel);
            
            createEvents = EditorGUILayout.Toggle("Game Events", createEvents);
            createVariables = EditorGUILayout.Toggle("Variables (Score, State)", createVariables);
            createConfig = EditorGUILayout.Toggle("Configuration (Settings, Scenes)", createConfig);
            createUI = EditorGUILayout.Toggle("UI States & Navigation", createUI);
            createRuntimeData = EditorGUILayout.Toggle("Runtime Data Container", createRuntimeData);
            
            EditorGUILayout.Space(20);
            
            if (GUILayout.Button("Create All Assets", GUILayout.Height(40)))
            {
                CreateAssets();
            }
            
            EditorGUILayout.Space(10);
            
            if (GUILayout.Button("Open Documentation"))
            {
                string docPath = "Assets/Scripts/ScriptableObjects/README.md";
                if (File.Exists(docPath))
                {
                    AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<Object>(docPath));
                }
                else
                {
                    EditorUtility.DisplayDialog("Documentation Not Found", 
                        "README.md not found at " + docPath, "OK");
                }
            }
        }
        
        private void CreateAssets()
        {
            // Create folder structure
            CreateFolderIfNeeded(BASE_PATH);
            CreateFolderIfNeeded(BASE_PATH + "/Events");
            CreateFolderIfNeeded(BASE_PATH + "/Variables");
            CreateFolderIfNeeded(BASE_PATH + "/Config");
            CreateFolderIfNeeded(BASE_PATH + "/UI");
            
            int created = 0;
            
            // Create Events
            if (createEvents)
            {
                created += CreateEventAsset("OnStartGameRequest", "Raised when the player requests to start the game");
                created += CreateEventAsset("OnPauseRequest", "Raised when the player requests to pause");
                created += CreateEventAsset("OnResumeRequest", "Raised when the player requests to resume");
                created += CreateEventAsset("OnRestartRequest", "Raised when the player requests to restart");
                created += CreateEventAsset("OnQuitRequest", "Raised when the player requests to quit");
                created += CreateEventAsset("OnMainMenuRequest", "Raised when the player requests to go to main menu");
                created += CreateEventAsset("OnPlayerDeath", "Raised when the player dies");
                created += CreateEventAsset("OnGameStarted", "Raised when the game actually starts");
                created += CreateEventAsset("OnGamePaused", "Raised when the game is paused");
                created += CreateEventAsset("OnGameResumed", "Raised when the game is resumed");
                created += CreateEventAsset("OnGameOver", "Raised when the game is over");
            }
            
            // Create Variables
            if (createVariables)
            {
                created += CreateIntVariableAsset("Score", "Current player score", 0);
                created += CreateIntVariableAsset("HighScore", "Highest score achieved", 0);
                created += CreateBoolVariableAsset("IsPaused", "Is the game currently paused?", false);
                created += CreateBoolVariableAsset("IsGameOver", "Is the game over?", false);
            }
            
            // Create Config
            if (createConfig)
            {
                created += CreateAssetIfNeeded<GameSettings>(BASE_PATH + "/Config/GameSettings.asset");
                created += CreateAssetIfNeeded<AudioSettings>(BASE_PATH + "/Config/AudioSettings.asset");
                created += CreateAssetIfNeeded<SceneReference>(BASE_PATH + "/Config/MainMenuScene.asset");
                created += CreateAssetIfNeeded<SceneReference>(BASE_PATH + "/Config/GameScene.asset");
            }
            
            // Create UI
            if (createUI)
            {
                created += CreatePanelStateAsset("MainMenuPanel", "Main menu screen");
                created += CreatePanelStateAsset("OptionsPanel", "Options/settings screen");
                created += CreatePanelStateAsset("PausePanel", "Pause menu during gameplay");
                created += CreatePanelStateAsset("GameOverPanel", "Game over screen");
                created += CreateAssetIfNeeded<UINavigationChannel>(BASE_PATH + "/UI/UINavigation.asset");
            }
            
            // Create Runtime Data
            if (createRuntimeData)
            {
                created += CreateAssetIfNeeded<GameRuntimeData>(BASE_PATH + "/GameRuntimeData.asset");
            }
            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            EditorUtility.DisplayDialog("Setup Complete", 
                $"Created {created} new ScriptableObject assets.\n\n" +
                "Next steps:\n" +
                "1. Configure scene references in Config folder\n" +
                "2. Wire up GameRuntimeData with all assets\n" +
                "3. Add GameManagerSO to your scene\n" +
                "4. See README.md for detailed setup guide",
                "OK");
        }
        
        private void CreateFolderIfNeeded(string path)
        {
            if (!AssetDatabase.IsValidFolder(path))
            {
                string parent = Path.GetDirectoryName(path);
                string folder = Path.GetFileName(path);
                AssetDatabase.CreateFolder(parent, folder);
            }
        }
        
        private int CreateAssetIfNeeded<T>(string path) where T : ScriptableObject
        {
            if (AssetDatabase.LoadAssetAtPath<T>(path) != null)
            {
                return 0; // Already exists
            }
            
            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return 1;
        }
        
        private int CreateEventAsset(string name, string description)
        {
            string path = $"{BASE_PATH}/Events/{name}.asset";
            if (AssetDatabase.LoadAssetAtPath<GameEvent>(path) != null)
            {
                return 0;
            }
            
            GameEvent asset = ScriptableObject.CreateInstance<GameEvent>();
            // Note: description field is private, would need reflection or public setter
            AssetDatabase.CreateAsset(asset, path);
            return 1;
        }
        
        private int CreateIntVariableAsset(string name, string description, int initialValue)
        {
            string path = $"{BASE_PATH}/Variables/{name}.asset";
            if (AssetDatabase.LoadAssetAtPath<IntVariable>(path) != null)
            {
                return 0;
            }
            
            IntVariable asset = ScriptableObject.CreateInstance<IntVariable>();
            AssetDatabase.CreateAsset(asset, path);
            return 1;
        }
        
        private int CreateBoolVariableAsset(string name, string description, bool initialValue)
        {
            string path = $"{BASE_PATH}/Variables/{name}.asset";
            if (AssetDatabase.LoadAssetAtPath<BoolVariable>(path) != null)
            {
                return 0;
            }
            
            BoolVariable asset = ScriptableObject.CreateInstance<BoolVariable>();
            AssetDatabase.CreateAsset(asset, path);
            return 1;
        }
        
        private int CreatePanelStateAsset(string name, string description)
        {
            string path = $"{BASE_PATH}/UI/{name}.asset";
            if (AssetDatabase.LoadAssetAtPath<UIPanelState>(path) != null)
            {
                return 0;
            }
            
            UIPanelState asset = ScriptableObject.CreateInstance<UIPanelState>();
            AssetDatabase.CreateAsset(asset, path);
            return 1;
        }
    }
}
#endif
