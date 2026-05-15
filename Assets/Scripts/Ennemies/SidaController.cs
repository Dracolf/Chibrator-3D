using UnityEngine;

public class SidaController : MonoBehaviour
{
    [SerializeField]
    private int avgSpeed = 15;

    [SerializeField]
    private Transform visual;

    [SerializeField]
    private float rotationSpeed = 180f;

    [SerializeField]
    private Vector3 rotationAxis = new Vector3(1f, 1f, 0.5f);

    [SerializeField]
    private int damage = 99;

    private int speed;

    private void Start()
    {
        speed = Random.Range(avgSpeed - 2, avgSpeed + 3);
        rotationAxis.Normalize();
    }

    private void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;

        if (visual != null)
        {
            visual.Rotate(rotationAxis * rotationSpeed * Time.deltaTime, Space.Self);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerController.ChangeHealth(-damage);
            int nbSidaInfections = PlayerPrefs.GetInt("sidaInfections");
            PlayerPrefs.SetInt("sidaInfections", nbSidaInfections + 1);
            Destroy(gameObject);
            return;
        }

        if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}