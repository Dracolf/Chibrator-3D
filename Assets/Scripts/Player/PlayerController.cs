using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;

    [SerializeField]
    private float mouseLookSensitivity = 0.2f;

    [SerializeField]
    private float gamepadLookSensitivity = 200f;

    [SerializeField]
    private HealthBarUI healthBar;

    private float maxHealth = 100f;
    private float currentHealth;

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
        Vector2 lookInput = playerInputController.LookInputVector;

        float currentSensitivity = playerInputController.IsMouseLook
            ? mouseLookSensitivity
            : gamepadLookSensitivity;

        float yaw = lookInput.x * currentSensitivity * Time.deltaTime;
        transform.Rotate(0f, yaw, 0f);

        Vector2 moveInput = playerInputController.MovementInputVector;

        Vector3 moveDirection =
            transform.right * moveInput.x +
            transform.forward * moveInput.y;

        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        transform.position += moveDirection * speed * Time.deltaTime;
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