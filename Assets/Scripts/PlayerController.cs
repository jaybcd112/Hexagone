using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public bool useRigidbodyMovement = false;
    public int percentage = 0;
    public int lives = 3;
    public float speed = 4f;
    public float jumpForce = 4f;
    public float groundedRaycastDistance = 0.1f;
    public float rotationSpeed = 1f;

    [Header("Components")]
    public Rigidbody rb;
    public Transform cameraAngle;
    public LayerMask groundLayer;
    public SkinnedMeshRenderer skinnedMeshRenderer;
    public GameObject deathYellSFX;
    public Collider hitBox;

    [Header("Audio Sources")]
    public AudioSource LightAttack;
    public AudioSource HeavyAttack;
    public AudioSource DeathRah;

    //private variables
    private Vector2 moveVector = Vector2.zero;
    private bool isGrounded;
    private Animator animator;
    private Quaternion targetRotation;
    private bool canAttack;
    private float speedMultiplier = 1f;
    private ParticleSystem ps;

    [HideInInspector]
    public TextMeshProUGUI playerIconText;
    public Image[] healthIcons;


    public void Awake()
    {
        canAttack = true;
        animator = GetComponent<Animator>();
        string playerName = gameObject.name;
        string currentPlayer = "Player" + playerName.Substring(6);
        GameObject playerIcon = GameObject.Find("Canvas/" + currentPlayer + "Icon");
        playerIconText = playerIcon.transform.Find("Current %").GetComponent<TextMeshProUGUI>();
        ps = GetComponent<ParticleSystem>();
        SetPlayerColor(currentPlayer);
    }

    public void OnMovementPerformed(InputAction.CallbackContext value)
    {
        moveVector = value.ReadValue<Vector2>();

    }

    public void OnLookPerformed(InputAction.CallbackContext value)
    {

        Vector2 temp = value.ReadValue<Vector2>();

        if (temp.magnitude < 0.1f)
        {
            temp = Vector2.zero;
        }

        if (temp != Vector2.zero)
        {
            float angle = Mathf.Atan2(temp.x, temp.y) * Mathf.Rad2Deg;

            float targetYRotation = cameraAngle.eulerAngles.y + angle;

            targetYRotation %= 360;
            if (targetYRotation < 0)
                targetYRotation += 360;

            targetRotation = Quaternion.Euler(0f, targetYRotation, 0f);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void OnJumpPerformed(InputAction.CallbackContext value)
    {
        if (isGrounded)
        {
            isGrounded = false;
            rb.AddForce(Vector3.up * jumpForce * 100, ForceMode.Impulse);

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, groundedRaycastDistance, groundLayer))
            {
                if (hit.collider.tag == "GlassTile")
                {
                    hit.collider.gameObject.GetComponent<GlassTile>()?.JumpImpact();
                }
            }
        }
    }


    public void OnLightHitPerformed(InputAction.CallbackContext value)
    {
        if (canAttack)
        {
            StartCoroutine(AttackCooldown(1f));
            animator.Play("LightAttack");
            LightAttack.Play();
        }
    }

    public void OnHeavyHitPerformed(InputAction.CallbackContext value)
    {
        if (canAttack)
        {
            StartCoroutine(AttackCooldown(1.2f));
            animator.Play("HeavyAttack");
            HeavyAttack.Play();
        }
    }

    private IEnumerator AttackCooldown(float clipLength)
    {
        canAttack = false;

        yield return new WaitForSeconds(.2f);

        hitBox.enabled = true;

        yield return new WaitForSeconds(clipLength);

        hitBox.enabled = false;
        canAttack = true;

    }

    private void Update()
    {
        Vector3 localMoveVector = transform.InverseTransformDirection(new Vector3(moveVector.x, 0f, moveVector.y));
        animator.SetFloat("x", localMoveVector.x);
        animator.SetFloat("y", localMoveVector.z);

        animator.SetBool("isGrounded", isGrounded);
        //Debug.Log(isGrounded);
    }

    private void FixedUpdate()
    {

        

        if (useRigidbodyMovement && rb.velocity.magnitude <= 5f)
        {
            Debug.Log("Velocity" + rb.velocity);
            Debug.Log("Magnitude" + rb.velocity.magnitude);
            Vector3 movement = new Vector3(moveVector.x, 0f, moveVector.y) * speed * .1f * speedMultiplier;
            Vector3 velocityChange = new Vector3(movement.x, 0f, movement.z);

            rb.velocity += velocityChange;
        }

        
        else
        {
            Vector3 movement = new Vector3(moveVector.x, 0f, moveVector.y) * speed * speedMultiplier;
            transform.Translate(movement * Time.deltaTime, Space.World);
        }
        

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, groundedRaycastDistance + 0.1f, groundLayer))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }


    public void UpdatePercentage(int newPercentage)
    {
        percentage += newPercentage;
        playerIconText.text = percentage.ToString() + "%";
    }

    public void ResetPercentage()
    {
        percentage = 0;
        playerIconText.text = percentage.ToString() + "%";
    }

    public int GetLives()
    {
        return lives;
    }

    public void UpdateLives(int newLives)
    {
        lives += newLives;

        healthIcons[(lives + 1)].enabled = false;

        ResetPercentage();

    }

    public void SetPlayerColor(string currentPlayer)
    {
        var main = ps.main;
        switch (currentPlayer)
        {
            case "Player1":
                skinnedMeshRenderer.material = Resources.Load<Material>("Materials/Player1");
                main.startColor = new Color(1f, 0f, 0f, 1f);
                break;
            case "Player2":
                skinnedMeshRenderer.material = Resources.Load<Material>("Materials/Player2");
                main.startColor = new Color(1f, 0f, 1f, 1f);
                break;
            case "Player3":
                skinnedMeshRenderer.material = Resources.Load<Material>("Materials/Player3");
                main.startColor = new Color(0f, 1f, 1f, 1f);
                break;
            case "Player4":
                skinnedMeshRenderer.material = Resources.Load<Material>("Materials/Player4");
                main.startColor = new Color(0f, 1f, 0f, 1f);
                break;
            default:
                Debug.LogError("Invalid player number!");
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DesertTile"))
        {
            speedMultiplier = 0.5f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DesertTile"))
        {
            speedMultiplier = 1f;
        }

    }

    public void Pause()
    {
        GameObject.Find("Canvas/PauseMenu").SetActive(true);
    }
}
