using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveForward : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        // Enable interpolation to smooth movement between physics steps
        // This prevents jitter when camera follows in LateUpdate
        if (_rb.interpolation == RigidbodyInterpolation2D.None)
        {
            _rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
    }

    private void FixedUpdate()
    {
        // Set constant horizontal velocity while preserving vertical velocity
        _rb.linearVelocity = new Vector2(moveSpeed, _rb.linearVelocity.y);
    }
}

