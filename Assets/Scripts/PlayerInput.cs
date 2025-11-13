using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private BirdyFlapInputs _inputActions;
    private Flapper _flapper;

    private void Awake()
    {
        _inputActions = new BirdyFlapInputs();
        _flapper = GetComponent<Flapper>();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Player.Jump.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        _inputActions.Player.Jump.performed -= OnJumpPerformed;
        _inputActions.Disable();
    }

    private void OnDestroy()
    {
        _inputActions?.Dispose();
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        _flapper.Flap();
    }
}

