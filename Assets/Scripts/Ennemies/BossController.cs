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
    private BossSpawner bossSpawner;
    private TerroristeSpawner terroristeSpawner;


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
        bossSpawner = FindAnyObjectByType<BossSpawner>();
        terroristeSpawner = FindAnyObjectByType<TerroristeSpawner>();
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
        BodyPillowController bodyPillow = collision.gameObject.GetComponent<BodyPillowController>();

        if (player != null)
        {
            player.ChangeHealth(-2);
        }
        if (bodyPillow != null)
        {
            bodyPillow.TakeDamage(2f);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (infos != null)
        {
            infos.SetHealth(health);
        }

        soundEffectPlayer.PlaySound(SoundEffectType.BossHit);

        if (health <= 0)
        {
            Score score = FindAnyObjectByType<Score>();
            score.IncreaseScore(100);
            int nbBossKills = PlayerPrefs.GetInt("bossKilled");
            int nbEnemyKills = PlayerPrefs.GetInt("enemiesKilled");
            PlayerPrefs.SetInt("bossKilled", nbBossKills + 1);
            PlayerPrefs.SetInt("enemiesKilled", nbEnemyKills + 1);
            PlayerPrefs.Save();

            if (score.score % 200 == 0)
            {
                if (itemDrop != null)
                {
                    itemDrop.TryDropBonus();
                }
            }

            if (score.score % 400 == 0)
            {
                terroristeSpawner.SpawnTerroriste();
            }

            if (score.score % 1000 == 0)
            {
                bossSpawner.SpawnBoss();
            }
            
            soundEffectPlayer.PlaySound(SoundEffectType.BossDeath);
            Destroy(gameObject);
        }
    }
}