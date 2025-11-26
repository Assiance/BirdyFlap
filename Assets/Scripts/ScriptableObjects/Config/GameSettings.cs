using UnityEngine;

namespace BirdyFlap.Config
{
    /// <summary>
    /// Central game configuration ScriptableObject.
    /// Contains all tunable game parameters in one place.
    /// Create via Assets > Create > BirdyFlap > Config > Game Settings
    /// </summary>
    [CreateAssetMenu(fileName = "GameSettings", menuName = "BirdyFlap/Config/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Player Settings")]
        [Tooltip("Force applied when the player flaps/jumps")]
        [SerializeField] private float jumpForce = 5f;
        
        [Tooltip("Gravity scale for the player's rigidbody")]
        [SerializeField] private float gravityScale = 1f;
        
        [Header("Obstacle Settings")]
        [Tooltip("Minimum time between obstacle spawns")]
        [SerializeField] private float minSpawnInterval = 2f;
        
        [Tooltip("Maximum time between obstacle spawns")]
        [SerializeField] private float maxSpawnInterval = 4f;
        
        [Tooltip("Distance ahead of player to spawn obstacles")]
        [SerializeField] private float spawnDistanceAhead = 18f;
        
        [Tooltip("Distance behind player to cleanup obstacles")]
        [SerializeField] private float cleanupDistanceBehind = 10f;
        
        [Header("Gap Settings")]
        [Tooltip("Minimum gap size for paired obstacles")]
        [SerializeField] private float minGapSize = 3.5f;
        
        [Tooltip("Maximum gap size for paired obstacles")]
        [SerializeField] private float maxGapSize = 6f;
        
        [Tooltip("Maximum screen coverage for single obstacles (0-1)")]
        [Range(0f, 1f)]
        [SerializeField] private float maxSingleCoverage = 0.66f;
        
        [Header("Game Speed")]
        [Tooltip("Base forward movement speed")]
        [SerializeField] private float baseSpeed = 5f;
        
        [Tooltip("Speed increase per score milestone")]
        [SerializeField] private float speedIncreasePerMilestone = 0.5f;
        
        [Tooltip("Score milestone interval for speed increases")]
        [SerializeField] private int speedMilestoneInterval = 10;
        
        // Public Properties - Read-only access
        public float JumpForce => jumpForce;
        public float GravityScale => gravityScale;
        public float MinSpawnInterval => minSpawnInterval;
        public float MaxSpawnInterval => maxSpawnInterval;
        public float SpawnDistanceAhead => spawnDistanceAhead;
        public float CleanupDistanceBehind => cleanupDistanceBehind;
        public float MinGapSize => minGapSize;
        public float MaxGapSize => maxGapSize;
        public float MaxSingleCoverage => maxSingleCoverage;
        public float BaseSpeed => baseSpeed;
        public float SpeedIncreasePerMilestone => speedIncreasePerMilestone;
        public int SpeedMilestoneInterval => speedMilestoneInterval;
        
        /// <summary>
        /// Calculates the current game speed based on score.
        /// </summary>
        public float GetSpeedForScore(int score)
        {
            int milestones = score / speedMilestoneInterval;
            return baseSpeed + (milestones * speedIncreasePerMilestone);
        }
        
        /// <summary>
        /// Gets a random spawn interval within the configured range.
        /// </summary>
        public float GetRandomSpawnInterval()
        {
            return Random.Range(minSpawnInterval, maxSpawnInterval);
        }
        
        /// <summary>
        /// Gets a random gap size within the configured range.
        /// </summary>
        public float GetRandomGapSize()
        {
            return Mathf.Max(minGapSize, Random.Range(minGapSize, maxGapSize));
        }
    }
}
