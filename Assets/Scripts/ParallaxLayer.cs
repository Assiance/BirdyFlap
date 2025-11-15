using UnityEngine;

/// <summary>
/// Creates a parallax scrolling effect by moving background layers at different speeds.
/// Supports endless scrolling by repositioning segments when they move off-screen.
/// </summary>
public class ParallaxLayer : MonoBehaviour
{
    [Header("Parallax Settings")]
    [SerializeField, Range(0f, 1f), Tooltip("How fast this layer moves relative to camera. 0 = static, 1 = moves with camera")]
    private float parallaxFactor = 0.5f;
    
    [Header("Endless Scrolling")]
    [SerializeField, Tooltip("Width of a single background segment")]
    private float segmentWidth = 20f;
    
    [SerializeField, Tooltip("Number of segments to use for seamless looping (2-3 recommended)")]
    private int segmentCount = 3;
    
    [Header("References")]
    [SerializeField, Tooltip("Leave empty to auto-find main camera")]
    private Camera mainCamera;
    
    private Transform[] segments;
    private float leftBoundary;
    private float rightBoundary;

    private void Awake()
    {
        // Auto-find camera if not assigned
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
        if (mainCamera == null)
        {
            Debug.LogError("ParallaxLayer: No camera found! Please assign a camera or tag one as MainCamera.");
            enabled = false;
            return;
        }
        
        InitializeSegments();
    }

    private void InitializeSegments()
    {
        // Get all child transforms as segments
        segments = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            segments[i] = transform.GetChild(i);
        }
        
        if (segments.Length == 0)
        {
            Debug.LogWarning($"ParallaxLayer on {gameObject.name}: No child segments found. Please add sprite GameObjects as children.");
            return;
        }
        
        // Position segments side by side
        for (int i = 0; i < segments.Length; i++)
        {
            Vector3 pos = segments[i].localPosition;
            pos.x = i * segmentWidth;
            segments[i].localPosition = pos;
        }
        
        // Calculate boundaries based on camera view and parallax factor
        UpdateBoundaries();
    }

    private void UpdateBoundaries()
    {
        if (mainCamera == null) return;
        
        // Calculate camera view width
        float cameraHeight = mainCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        
        // Add buffer to ensure segments are repositioned before they're visible
        float buffer = segmentWidth * 0.5f;
        
        leftBoundary = -cameraWidth * 0.5f - buffer;
        rightBoundary = cameraWidth * 0.5f + buffer;
    }

    private void LateUpdate()
    {
        if (mainCamera == null || segments.Length == 0) return;
        
        // Calculate absolute position based on camera position and parallax factor
        float currentCameraX = mainCamera.transform.position.x;
        float targetX = currentCameraX * parallaxFactor;
        
        // Set position directly (no delta calculation to avoid jitter)
        Vector3 newPos = transform.position;
        newPos.x = targetX;
        transform.position = newPos;
        
        // Check each segment for repositioning
        RepositionSegments();
    }

    private void RepositionSegments()
    {
        if (mainCamera == null) return;
        
        float cameraX = mainCamera.transform.position.x;
        
        // Find the rightmost segment position BEFORE any repositioning
        float maxX = float.MinValue;
        foreach (Transform segment in segments)
        {
            if (segment.position.x > maxX)
            {
                maxX = segment.position.x;
            }
        }
        
        // Now check each segment and reposition if needed
        foreach (Transform segment in segments)
        {
            // Get segment's world position
            float segmentWorldX = segment.position.x;
            
            // Calculate relative position to camera
            float relativeX = segmentWorldX - cameraX;
            
            // If segment is too far left, move it to the right
            if (relativeX < leftBoundary)
            {
                // Position this segment to the right of the rightmost one
                Vector3 newPos = segment.position;
                newPos.x = maxX + segmentWidth;
                segment.position = newPos;
                
                // Update maxX for the next segment that might need repositioning
                maxX = newPos.x;
            }
        }
    }

    private void OnValidate()
    {
        // Clamp segment count to reasonable values
        segmentCount = Mathf.Clamp(segmentCount, 2, 10);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        // Visualize segment boundaries in editor
        if (segments != null && segments.Length > 0)
        {
            Gizmos.color = Color.yellow;
            foreach (Transform segment in segments)
            {
                if (segment != null)
                {
                    Vector3 center = segment.position;
                    Vector3 size = new Vector3(segmentWidth, 10f, 0f);
                    Gizmos.DrawWireCube(center, size);
                }
            }
        }
    }
#endif
}

