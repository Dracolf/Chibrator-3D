using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;

    public float projectileDamage = 5f;
    public float damageMultiplier = 1f;

    [Header("Look")]
    [SerializeField]
    private float mouseLookSensitivity = 0.2f;

    [SerializeField]
    private float gamepadLookSensitivity = 200f;

    [SerializeField]
    private Transform cameraPivot;

    [SerializeField]
    private float minCameraPitch = -70f;

    [SerializeField]
    private float maxCameraPitch = 70f;

    private float cameraPitch;

    public float maxHealth = 100f;
    public float currentHealth;

    [SerializeField]
    private HealthBarUI healthBar;

    [Header("Map Limits")]
    [SerializeField]
    private float mapHalfSize = 37.5f;

    [SerializeField]
    private float playerBoundaryMargin = 0.7f;

    private PlayerInputController playerInputController;

    private void Awake()
    {
        playerInputController = GetComponent<PlayerInputController>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleLook();
        HandleMovement();
    }

    private void HandleLook()
    {
        Vector2 lookInput = playerInputController.LookInputVector;

        float currentSensitivity = playerInputController.IsMouseLook
            ? mouseLookSensitivity
            : gamepadLookSensitivity;

        float yaw = lookInput.x * currentSensitivity * Time.deltaTime;
        transform.Rotate(0f, yaw, 0f);

        float pitch = lookInput.y * currentSensitivity * Time.deltaTime;

        cameraPitch -= pitch;
        cameraPitch = Mathf.Clamp(cameraPitch, minCameraPitch, maxCameraPitch);

        if (cameraPivot != null)
        {
            cameraPivot.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }
    }

    private void HandleMovement()
    {
        Vector2 moveInput = playerInputController.MovementInputVector;

        Vector3 moveDirection =
            transform.right * moveInput.x +
            transform.forward * moveInput.y;

        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        Vector3 nextPosition = transform.position + moveDirection * speed * Time.deltaTime;

        float minPosition = -mapHalfSize + playerBoundaryMargin;
        float maxPosition = mapHalfSize - playerBoundaryMargin;

        nextPosition.x = Mathf.Clamp(nextPosition.x, minPosition, maxPosition);
        nextPosition.z = Mathf.Clamp(nextPosition.z, minPosition, maxPosition);

        transform.position = nextPosition;
    }

    public void ChangeHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0f)
        {
            Score score = FindAnyObjectByType<Score>();

            PlayerPrefs.SetInt("LastScore", score.score);

            if (score.score > PlayerPrefs.GetInt("HighScore", 0))
            {
                PlayerPrefs.SetInt("HighScore", score.score);
            }

            PlayerPrefs.Save();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("Menu");
        }
    }
}