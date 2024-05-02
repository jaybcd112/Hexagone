using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls input = null;
    private Vector2 moveVector = Vector2.zero;
    public Transform cameraAngle;
    public float speed;
    public float jumpForce;
    public float groundedRaycastDistance = 0.1f; // Distance to check for ground
    public LayerMask groundLayer; // Layer mask for the ground objects
    private float speedMultiplier = 1f;
    private Rigidbody rb;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    public void OnMovementCanceled(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    public void OnSprintPerformed(InputAction.CallbackContext value)
    {
        speedMultiplier = 1.5f;
    }

    public void OnSprintCanceled(InputAction.CallbackContext value)
    {
        speedMultiplier = 1f;
    }

    public void OnJumpPerformed(InputAction.CallbackContext value)
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); // Apply jump force only when grounded
        }
    }

    public void OnJumpCanceled(InputAction.CallbackContext value)
    {
        // add jump cancel logic here if needed
    }

    private void FixedUpdate()
    {
        float moveHorizontal = moveVector.x;
        float moveVertical = moveVector.y;

        // Calculate camera forward and right vectors relative to player rotation
        Vector3 cameraForward = Quaternion.Euler(0, cameraAngle.eulerAngles.y, 0) * Vector3.forward;
        Vector3 cameraRight = Quaternion.Euler(0, cameraAngle.eulerAngles.y, 0) * Vector3.right;
        
        Vector3 movement = moveHorizontal * cameraRight + moveVertical * cameraForward;
        
        movement = movement.normalized * Time.deltaTime * speed * speedMultiplier;
        
        transform.Translate(movement);

        // Check if the player is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundedRaycastDistance, groundLayer);

        //Debug.Log(moveVector);
        //Debug.Log(movement);
        Debug.Log("Grounded: " + isGrounded);
    }
}