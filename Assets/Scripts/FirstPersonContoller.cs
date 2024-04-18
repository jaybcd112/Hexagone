using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FirstPersonController : MonoBehaviour
{
    private Vector3 initialLocation;
    public float moveSpeed = 2f;

    public Transform cameraDamper;
    public float sprintSpeed = 5f;
    public float jumpForce = 5f;
    public float mouseSensitivity = 500f;
    public float gravity = 9.81f; // Gravity strength

    private float verticalRotation = 0f;
    private float verticalVelocity = 0f; // Current vertical velocity for gravity application
    private bool isGrounded;

    private CharacterController characterController;

    void Start()
    {
        initialLocation = transform.position;
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked; // Lock cursor to center of screen
    }

    void Update()
    {
        // Check grounded status at the start of each frame
        isGrounded = characterController.isGrounded;
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = 0f;
        }

        // Rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -70f, 70f);

        transform.Rotate(Vector3.up * mouseX);
        cameraDamper.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);

        // Movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        // Sprint
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        // Apply gravity
        if (!isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            verticalVelocity = jumpForce;
        }

        // Final move vector
        Vector3 finalMove = (move * currentSpeed) + Vector3.up * verticalVelocity;
        characterController.Move(finalMove * Time.deltaTime);

        //reset the ball by dropping it to initial location
        if (Input.GetKey(KeyCode.R))
        {
            transform.position = initialLocation;
        }

    }
}
