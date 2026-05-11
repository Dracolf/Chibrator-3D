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
    private CameraEffects cameraEffects;

    [Header("Sounds")]
    [SerializeField]
    private AudioSource footstepsAudioSource;

    [SerializeField]
    private AudioSource hitAudioSource;

    [SerializeField]
    private AudioClip footSteps;

    [SerializeField]
    private AudioClip footStepsSprint;

    [SerializeField]
    private float hitSoundStopDelay = 0.2f;

    private float lastDamageTime;

    private void Awake()
    {
        playerInputController = GetComponent<PlayerInputController>();
        cameraEffects = FindAnyObjectByType<CameraEffects>();

        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (IsGamePaused())
        {
            StopGameplaySounds();
            return;
        }

        HandleLook();
        HandleMovement();
        ManageHitSound();
    }

    private bool IsGamePaused()
    {
        return PauseManager.Instance != null && PauseManager.Instance.IsPaused;
    }

    private void HandleLook()
    {
        if (playerInputController == null)
        {
            return;
        }

        Vector2 lookInput = playerInputController.LookInputVector;

        float yaw;
        float pitch;

        if (playerInputController.IsMouseLook)
        {
            yaw = lookInput.x * mouseLookSensitivity;
            pitch = lookInput.y * mouseLookSensitivity;
        }
        else
        {
            yaw = lookInput.x * gamepadLookSensitivity * Time.deltaTime;
            pitch = lookInput.y * gamepadLookSensitivity * Time.deltaTime;
        }

        transform.Rotate(0f, yaw, 0f);

        cameraPitch -= pitch;
        cameraPitch = Mathf.Clamp(cameraPitch, minCameraPitch, maxCameraPitch);

        if (cameraPivot != null)
        {
            cameraPivot.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
        }
    }

    private void HandleMovement()
    {
        if (playerInputController == null)
        {
            return;
        }

        Vector2 moveInput = playerInputController.MovementInputVector;

        Vector3 moveDirection =
            transform.right * moveInput.x +
            transform.forward * moveInput.y;

        bool isMoving = moveInput.magnitude > 0.01f;

        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        ManageFootstepsSound(isMoving);

        Vector3 nextPosition = transform.position + moveDirection * speed * Time.deltaTime;

        float minPosition = -mapHalfSize + playerBoundaryMargin;
        float maxPosition = mapHalfSize - playerBoundaryMargin;

        nextPosition.x = Mathf.Clamp(nextPosition.x, minPosition, maxPosition);
        nextPosition.z = Mathf.Clamp(nextPosition.z, minPosition, maxPosition);

        transform.position = nextPosition;
    }

    public void ChangeHealth(float amount)
    {
        if (amount < 0f && cameraEffects != null)
        {
            lastDamageTime = Time.time;

            if (hitAudioSource != null && !hitAudioSource.isPlaying)
            {
                hitAudioSource.Play();
            }

            cameraEffects.PlayDamageFlash();
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        StopGameplaySounds();

        Score score = FindAnyObjectByType<Score>();

        int finalScore = 0;

        if (score != null)
        {
            finalScore = score.score;
        }

        int currentMoney = PlayerPrefs.GetInt("money");

        PlayerPrefs.SetInt("LastScore", finalScore);
        PlayerPrefs.SetInt("money", currentMoney + finalScore);

        if (finalScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
        }

        PlayerPrefs.SetInt("nukeAmount", 0);
        PlayerPrefs.SetInt("pillowAmount", 0);

        PlayerPrefs.Save();

        Time.timeScale = 1f;
        AudioListener.pause = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("Menu");
    }

    private void ManageFootstepsSound(bool isMoving)
    {
        if (footstepsAudioSource == null)
        {
            return;
        }

        footstepsAudioSource.generator = speed > 10f ? footStepsSprint : footSteps;

        if (isMoving)
        {
            if (!footstepsAudioSource.isPlaying)
            {
                footstepsAudioSource.Play();
            }
        }
        else
        {
            if (footstepsAudioSource.isPlaying)
            {
                footstepsAudioSource.Stop();
            }
        }
    }

    private void ManageHitSound()
    {
        if (hitAudioSource == null || !hitAudioSource.isPlaying)
        {
            return;
        }

        if (Time.time > lastDamageTime + hitSoundStopDelay)
        {
            hitAudioSource.Stop();
        }
    }

    private void StopGameplaySounds()
    {
        if (footstepsAudioSource != null && footstepsAudioSource.isPlaying)
        {
            footstepsAudioSource.Stop();
        }

        if (hitAudioSource != null && hitAudioSource.isPlaying)
        {
            hitAudioSource.Stop();
        }
    }
}