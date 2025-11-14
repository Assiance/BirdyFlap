using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Flapper : MonoBehaviour
{
    [SerializeField] private float jumpForce = 5f;
    
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Flap()
    {
        // Zero out Y velocity for consistent jump height
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0f);
        
        // Apply upward force
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}

