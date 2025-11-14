using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveForward : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Set constant horizontal velocity while preserving vertical velocity
        _rb.linearVelocity = new Vector2(moveSpeed, _rb.linearVelocity.y);
    }
}

