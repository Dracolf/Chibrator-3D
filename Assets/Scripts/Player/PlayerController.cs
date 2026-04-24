using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;

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
    }

    private void Update()
    {
        Vector3 positionChange = new Vector3(
            playerInputController.MovementInputVector.x,
            0,
            playerInputController.MovementInputVector.y)
            * Time.deltaTime
            * speed;

        transform.position += positionChange;
    }

    public void ChangeHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0f)
        {
            SceneManager.LoadScene("Menu");
        }
    }
}