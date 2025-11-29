using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadblockSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject roadblockPrefab;
    [SerializeField] private GameObject scoreColliderPrefab;
    [SerializeField] private Transform player;
    
    [Header("Spawn Timing")]
    [SerializeField] private float minSpawnInterval = 2f;
    [SerializeField] private float maxSpawnInterval = 4f;
    
    [Header("Spawn Positioning")]
    [SerializeField] private float spawnDistanceAhead = 18f;
    [SerializeField] private float cleanupDistanceBehind = 10f;
    
    [Header("Single Roadblock Settings")]
    [SerializeField] [Range(0f, 1f)] private float maxSingleCoverage = 0.66f;
    
    [Header("Pair Roadblock Settings")]
    [SerializeField] private float minGapSize = 3.5f;
    [SerializeField] private float pairGapMin = 3.5f;
    [SerializeField] private float pairGapMax = 6f;
    
    [Header("Score Collider Settings")]
    [SerializeField] private float scoreColliderDistance = 1f;
    
    private List<GameObject> spawnedRoadblocks = new List<GameObject>();
    private Camera mainCamera;
    private Coroutine spawnCoroutine;
    
    private void Start()
    {
        mainCamera = Camera.main;
        
        if (mainCamera == null)
        {
            Debug.LogError("RoadblockSpawner: No main camera found!");
            enabled = false;
            return;
        }
        
        if (roadblockPrefab == null)
        {
            Debug.LogError("RoadblockSpawner: Roadblock prefab not assigned!");
            enabled = false;
            return;
        }
        
        if (scoreColliderPrefab == null)
        {
            Debug.LogError("RoadblockSpawner: ScoreCollider prefab not assigned!");
            enabled = false;
            return;
        }
        
        if (player == null)
        {
            Debug.LogError("RoadblockSpawner: Player transform not assigned!");
            enabled = false;
            return;
        }
        
        // Start the spawn coroutine
        spawnCoroutine = StartCoroutine(SpawnRoutine());
    }
    
    private void LateUpdate()
    {
        CleanupOldRoadblocks();
    }
    
    private IEnumerator SpawnRoutine()
    {
        // Initial delay before first spawn
        yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));
        
        while (true)
        {
            SpawnRoadblock();
            
            // Wait for random interval before next spawn
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }
    
    private void SpawnRoadblock()
    {
        // Randomly decide between single or pair
        bool isPair = Random.value > 0.5f;
        
        // Calculate spawn X position
        float spawnX = player.position.x + spawnDistanceAhead;
        
        if (isPair)
        {
            SpawnPair(spawnX);
        }
        else
        {
            SpawnSingle(spawnX);
        }
    }
    
    private void SpawnSingle(float spawnX)
    {
        // Get screen bounds in world space
        float screenTop = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        float screenBottom = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float screenHeight = screenTop - screenBottom;
        
        // Randomly choose top or bottom
        bool placeAtTop = Random.value > 0.5f;
        
        // Calculate maximum coverage in world units
        float maxCoverage = screenHeight * maxSingleCoverage;
        
        // Random actual coverage (between 30% and max coverage for variety)
        float actualCoverage = Random.Range(screenHeight * 0.3f, maxCoverage);
        
        float yPosition;
        if (placeAtTop)
        {
            // Place at top, extending downward
            yPosition = screenTop - (actualCoverage * 0.5f);
        }
        else
        {
            // Place at bottom, extending upward
            yPosition = screenBottom + (actualCoverage * 0.5f);
        }
        
        Vector3 spawnPosition = new Vector3(spawnX, yPosition, 0);
        GameObject roadblock = Instantiate(roadblockPrefab, spawnPosition, Quaternion.identity);
        
        // Scale the roadblock to cover the desired amount
        Vector3 scale = roadblock.transform.localScale;
        scale.y = actualCoverage;
        roadblock.transform.localScale = scale;
        
        spawnedRoadblocks.Add(roadblock);
        
        // Spawn ScoreCollider in the open area behind the roadblock
        float openAreaCenter;
        if (placeAtTop)
        {
            // Roadblock at top, open area is at bottom
            openAreaCenter = screenBottom + (screenHeight - actualCoverage) * 0.5f;
        }
        else
        {
            // Roadblock at bottom, open area is at top
            openAreaCenter = screenTop - (screenHeight - actualCoverage) * 0.5f;
        }
        
        Vector3 scoreColliderPosition = new Vector3(spawnX + scoreColliderDistance, openAreaCenter, 0);
        GameObject scoreCollider = Instantiate(scoreColliderPrefab, scoreColliderPosition, Quaternion.identity);
        spawnedRoadblocks.Add(scoreCollider);
    }
    
    private void SpawnPair(float spawnX)
    {
        // Get screen bounds in world space
        float screenTop = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y;
        float screenBottom = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y;
        float screenHeight = screenTop - screenBottom;
        
        // Random gap size between min and max
        float gapSize = Random.Range(pairGapMin, pairGapMax);
        
        // Ensure gap is at least the minimum
        gapSize = Mathf.Max(gapSize, minGapSize);
        
        // Randomly position the gap vertically (center of gap can be anywhere that keeps both roadblocks visible)
        float minGapCenter = screenBottom + gapSize * 0.5f + 1f; // +1 for some padding
        float maxGapCenter = screenTop - gapSize * 0.5f - 1f; // -1 for some padding
        float gapCenter = Random.Range(minGapCenter, maxGapCenter);
        
        // Calculate top and bottom of the gap
        float gapTop = gapCenter + gapSize * 0.5f;
        float gapBottom = gapCenter - gapSize * 0.5f;
        
        // Calculate roadblock sizes
        float topRoadblockSize = screenTop - gapTop;
        float bottomRoadblockSize = gapBottom - screenBottom;
        
        // Spawn top roadblock
        float topYPosition = gapTop + topRoadblockSize * 0.5f;
        Vector3 topSpawnPosition = new Vector3(spawnX, topYPosition, 0);
        GameObject topRoadblock = Instantiate(roadblockPrefab, topSpawnPosition, Quaternion.identity);
        Vector3 topScale = topRoadblock.transform.localScale;
        topScale.y = topRoadblockSize;
        topRoadblock.transform.localScale = topScale;
        spawnedRoadblocks.Add(topRoadblock);
        
        // Spawn bottom roadblock
        float bottomYPosition = screenBottom + bottomRoadblockSize * 0.5f;
        Vector3 bottomSpawnPosition = new Vector3(spawnX, bottomYPosition, 0);
        GameObject bottomRoadblock = Instantiate(roadblockPrefab, bottomSpawnPosition, Quaternion.identity);
        Vector3 bottomScale = bottomRoadblock.transform.localScale;
        bottomScale.y = bottomRoadblockSize;
        bottomRoadblock.transform.localScale = bottomScale;
        spawnedRoadblocks.Add(bottomRoadblock);
        
        // Spawn ScoreCollider in the gap behind the roadblocks
        Vector3 scoreColliderPosition = new Vector3(spawnX + scoreColliderDistance, gapCenter, 0);
        GameObject scoreCollider = Instantiate(scoreColliderPrefab, scoreColliderPosition, Quaternion.identity);
        spawnedRoadblocks.Add(scoreCollider);
    }
    
    private void CleanupOldRoadblocks()
    {
        if (player == null) return;
        
        float cleanupThreshold = player.position.x - cleanupDistanceBehind;
        
        // Iterate backwards to safely remove items
        for (int i = spawnedRoadblocks.Count - 1; i >= 0; i--)
        {
            if (spawnedRoadblocks[i] == null)
            {
                // Already destroyed, just remove from list
                spawnedRoadblocks.RemoveAt(i);
            }
            else if (spawnedRoadblocks[i].transform.position.x < cleanupThreshold)
            {
                // Behind player, destroy and remove
                Destroy(spawnedRoadblocks[i]);
                spawnedRoadblocks.RemoveAt(i);
            }
        }
    }
    
    private void OnDestroy()
    {
        // Clean up coroutine
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
    }
}

