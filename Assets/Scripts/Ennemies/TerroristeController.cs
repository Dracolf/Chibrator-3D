using UnityEngine;

public class TerroristeController : MonoBehaviour
{
    private Transform target;
    public float health = 50f;

    [SerializeField]
    private int speed = 10;

    private ZiziInfosDisplay infos;
    private ItemDrop itemDrop;
    private SoundEffectPlayer soundEffectPlayer;
    private BossSpawner bossSpawner;
    private TerroristeSpawner terroristeSpawner;

    private bool hasExploded;

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
        if (target == null || hasExploded)
        {
            return;
        }

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

    private void OnCollisionEnter(Collision collision)
    {
        if (hasExploded)
        {
            return;
        }

        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        BodyPillowController bodyPillow = collision.gameObject.GetComponent<BodyPillowController>();

        if (player != null)
        {
            Explode(true);

            player.ChangeHealth(-100f);

            Destroy(gameObject);
            return;
        }

        if (bodyPillow != null)
        {
            Explode();

            bodyPillow.TakeDamage(100f);

            Destroy(gameObject);
        }
    }

    private void Explode(bool persistSoundAfterSceneLoad = false)
    {
        hasExploded = true;

        if (soundEffectPlayer != null)
        {
            if (persistSoundAfterSceneLoad)
            {
                soundEffectPlayer.PlaySoundPersistAfterSceneLoad(SoundEffectType.Boom);
            }
            else
            {
                soundEffectPlayer.PlaySound(SoundEffectType.Boom);
            }
        }

        CameraEffects cameraEffects = FindAnyObjectByType<CameraEffects>();

        if (cameraEffects != null)
        {
            cameraEffects.PlayShake(0.25f, 0.3f);
        }
    }

    public void TakeDamage(float amount)
    {
        if (hasExploded)
        {
            return;
        }

        health -= amount;

        if (infos != null)
        {
            infos.SetHealth(health);
        }

        if (soundEffectPlayer != null)
        {
            soundEffectPlayer.PlaySound(SoundEffectType.ZiziHit);
        }

        if (health <= 0)
        {
            Score score = FindAnyObjectByType<Score>();

            if (score != null)
            {
                score.IncreaseScore(40);
                int nbTerroristKills = PlayerPrefs.GetInt("terroristsKilled");
                int nbEnemyKills = PlayerPrefs.GetInt("enemiesKilled");
                PlayerPrefs.SetInt("terroristsKilled", nbTerroristKills + 1);
                PlayerPrefs.SetInt("enemiesKilled", nbEnemyKills + 1);
                PlayerPrefs.Save();

                if (score.score % 200 == 0 && itemDrop != null)
                {
                    itemDrop.TryDropBonus();
                }

                if (score.score % 400 == 0 && terroristeSpawner != null)
                {
                    terroristeSpawner.SpawnTerroriste();
                }

                if (score.score % 1000 == 0 && bossSpawner != null)
                {
                    bossSpawner.SpawnBoss();
                }
            }

            Explode();

            Destroy(gameObject);
        }
    }
}