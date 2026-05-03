using UnityEngine;

public class BossController : MonoBehaviour
{
    private Transform target;
    public float health = 250f;
    [SerializeField]
    private int speed = 3;

    private ZiziInfosDisplay infos;
    private ItemDrop itemDrop;
    private SoundEffectPlayer soundEffectPlayer;

    private void Start()
    {
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            target = player.transform;
        }

        infos = GetComponentInChildren<ZiziInfosDisplay>();
        if (infos != null)
        {
            infos.SetMaxHealth(health);
            infos.SetHealth(health);
        }
        itemDrop = GetComponent<ItemDrop>();
        soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();
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

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (infos != null)
        {
            infos.SetHealth(health);
        }

        if (health <= 0)
        {
            Score score = FindAnyObjectByType<Score>();
            score.IncreaseScore(100);
            if (score.score % 200 == 0)
            {
                int dropBonusOrNot = Random.Range(0,5);
                if (dropBonusOrNot < 4)
                {
                    itemDrop.DropBonus();
                }
            }
            soundEffectPlayer.PlaySound(SoundEffectType.BossDeath);
            Destroy(gameObject);
        }
    }
}