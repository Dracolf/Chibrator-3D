using UnityEngine;

public class ZiziController : MonoBehaviour
{
    private Transform target;

    [SerializeField]
    private int _avgSpeed = 3;

    public float health = 50f;
    public int speed;

    private ZiziInfosDisplay infos;
    private ItemDrop itemDrop;
    private BossSpawner bossSpawner;
    private TerroristeSpawner terroristeSpawner;
    private SoundEffectPlayer soundEffectPlayer;

    private void Start()
    {
        soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();
        RefreshTarget();

        speed = Random.Range(_avgSpeed - 1, _avgSpeed + 2);

        infos = GetComponentInChildren<ZiziInfosDisplay>();
        if (infos != null)
        {
            infos.SetMaxHealth(health);
            infos.SetHealth(health);
        }
        itemDrop = GetComponent<ItemDrop>();
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
        LeurreController leurre = collision.gameObject.GetComponent<LeurreController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
        if (bodyPillow != null)
        {
            bodyPillow.TakeDamage(1f);
        }
        if (leurre != null)
        {
            leurre.TakeDamage(1f);
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (infos != null)
        {
            infos.SetHealth(health);
        }

        soundEffectPlayer.PlaySound(SoundEffectType.ZiziHit);

        if (health <= 0)
        {
            Score score = FindAnyObjectByType<Score>();
            score.IncreaseScore(20);
            int nbZiziKills = PlayerPrefs.GetInt("zizisKilled");
            int nbEnemyKills = PlayerPrefs.GetInt("enemiesKilled");
            PlayerPrefs.SetInt("zizisKilled", nbZiziKills + 1);
            PlayerPrefs.SetInt("enemiesKilled", nbEnemyKills + 1);
            PlayerPrefs.Save();

            if (score.score % 200 == 0)
            {
                if (itemDrop != null)
                {
                    itemDrop.TryDropBonus();
                }
            }

            if (score.score % 400 == 0 && score.isScoringEnabled)
            {
                terroristeSpawner.SpawnTerroriste();
            }

            if (score.score % 1000 == 0 && score.isScoringEnabled)
            {
                bossSpawner.SpawnBoss();
            }
            soundEffectPlayer.PlaySound(SoundEffectType.ZiziDeath);
            Destroy(gameObject);
        }
    }

    public void RefreshTarget()
    {
        target = LeurreController.GetCurrentTarget();
    }
}