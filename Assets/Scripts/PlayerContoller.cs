using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveVector = Vector2.zero;
    public int percentage = 0;
    public int lives = 3;
    public float speed;
    public float jumpForce;
    public float groundedRaycastDistance = 0.1f; // Distance to check for ground
    public LayerMask groundLayer; // Layer mask for the ground objects
    private float speedMultiplier = 1f;
    public Rigidbody rb;
    private bool isGrounded;
    private Quaternion targetRotation;
    public Transform playerModel;
    public Transform cameraAngle;
    public TextMeshProUGUI announcementText;
    public TextMeshProUGUI playerIconText;
    public Image[] healthIcons;
    private Animator animator; // Reference to the Animator component

    public void Awake()
    {
        animator = GetComponent<Animator>();
        string playerName = gameObject.name;
        string currentPlayer = "Player" + playerName.Substring(6);
        GameObject playerIcon = GameObject.Find("Canvas/" + currentPlayer + "Icon");
        playerIconText = playerIcon.transform.Find("Current %").GetComponent<TextMeshProUGUI>();
        announcementText = GameObject.Find("Canvas/Announcement Text").GetComponent<TextMeshProUGUI>();
        setPlayerColor(currentPlayer);
    }

    public void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();
    }

    public void OnLookPerformed(InputAction.CallbackContext value)
    {
        Vector2 temp = value.ReadValue<Vector2>();
        float angle = Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg;
        targetRotation = Quaternion.Euler(0f, (-1f * angle) + cameraAngle.eulerAngles.y, 0f);
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

    public void OnHitPerformed(InputAction.CallbackContext value)
    {
        // Check if the animator is currently playing the "SwingDown" state
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!stateInfo.IsName("SwingDown"))
        {
            // Set the parameter to true if not in the "SwingDown" state
            animator.SetBool("AttackPressed", true);
        }
        else
        {
            // Set the parameter to false if in the "SwingDown" state
            animator.SetBool("AttackPressed", false);
        }
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

        playerModel.rotation = targetRotation;

        // Check if the player is grounded
        isGrounded = Physics.Raycast(playerModel.position, Vector3.down, groundedRaycastDistance, groundLayer);

        /*Debug.Log(moveVector);
        Debug.Log(movement);
        Debug.Log("Grounded: " + isGrounded);*/
    }

    public void updatePercentage(int newPercentage)
    {
        percentage += newPercentage;
        playerIconText.text = percentage.ToString() + "%";
    }

    public void resetPercentage()
    {
        percentage = 0;
        playerIconText.text = percentage.ToString() + "%";
    }

    public int getLives()
    {
        return lives;
    }

    public void updateLives(int newLives)
    {
        lives += newLives;

        healthIcons[(lives + 1)].enabled = false;

        resetPercentage();

    }

    public void setPlayerColor(string currentPlayer)
    {
        switch (currentPlayer)
        {
            case "Player1":
                this.gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Player1");
                break;
            case "Player2":
                this.gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Player2");
                break;
            case "Player3":
                this.gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Player3");
                break;
            case "Player4":
                this.gameObject.GetComponent<Renderer>().material = Resources.Load<Material>("Materials/Player4");
                break;
        }
    }

    public void dead()
    {
        announcementText.text = this.gameObject.name + " is dead";
        this.gameObject.SetActive(false);
    }

    public void pause()
    {
        GameObject.Find("Canvas/PauseMenu").SetActive(true);
    }
}