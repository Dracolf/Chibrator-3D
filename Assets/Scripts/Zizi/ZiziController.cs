using UnityEngine;

public class ZiziController : MonoBehaviour
{
    private Transform target;

    [SerializeField]
    private int _avgSpeed = 3;

    public int health = 50;
    public int speed;

    private ZiziInfosDisplay infos;

    private void Start()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            target = player.transform;
        }

        speed = Random.Range(_avgSpeed - 1, _avgSpeed + 2);

        infos = GetComponentInChildren<ZiziInfosDisplay>();
        if (infos != null)
        {
            infos.SetMaxHealth(health);
            infos.SetHealth(health);
        }
    }

    private void Update()
    {
        if (target == null)
            return;

        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                5f * Time.deltaTime
            );
        }

        Vector3 moveDirection = direction.normalized;
        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void OnCollisionStay(Collision collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;

        if (infos != null)
        {
            infos.SetHealth(health);
        }

        if (health <= 0)
        {
            Destroy(gameObject);
            Score score = FindAnyObjectByType<Score>();
            score.IncreaseScore(100);
        }
    }
}