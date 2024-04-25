using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls input = null;
    private Vector2 moveVector = Vector2.zero;

    private void start()
    {
        input = new PlayerControls();
    }

    private void onEnable()
    {
        input.Enable();
        input.Player.Movement.performed += OnMovementPerformed;
    }

    private void onDisable()
    {
        input.Disable();
        input.Player.Movement.performed -= OnMovementPerformed;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
        Debug.Log(moveVector);
    }

    private void FixedUpdate()
    {
        
    }
}
