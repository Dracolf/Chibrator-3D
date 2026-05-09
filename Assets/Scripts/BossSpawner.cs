using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _boss;

    [Header("Spawn Around Player")]
    [SerializeField]
    private float minSpawnDistanceFromPlayer = 16f;

    [SerializeField]
    private float maxSpawnDistanceFromPlayer = 24f;

    [SerializeField]
    private float spawnY = 3.4f;

    [Header("Map Limits")]
    [SerializeField]
    private float mapHalfSize = 37.5f;

    [SerializeField]
    private float spawnBoundaryMargin = 3f;

    private SoundEffectPlayer soundEffectPlayer;
    private Transform playerTransform;

    private void Start()
    {
        soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();

        PlayerController playerController = FindAnyObjectByType<PlayerController>();

        if (playerController != null)
        {
            playerTransform = playerController.transform;
        }
    }

    public void SpawnBoss()
    {
        Vector3 spawnPosition = GetSpawnPositionAroundPlayer();

        Instantiate(
            _boss,
            spawnPosition,
            Quaternion.identity
        );

        if (soundEffectPlayer != null)
        {
            soundEffectPlayer.PlaySound(SoundEffectType.BossSpawn);
        }
    }

    private Vector3 GetSpawnPositionAroundPlayer()
    {
        if (playerTransform == null)
        {
            return new Vector3(
                Random.Range(-25f, 25f),
                spawnY,
                Random.Range(-25f, 25f)
            );
        }

        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minSpawnDistanceFromPlayer, maxSpawnDistanceFromPlayer);

        Vector3 spawnOffset = new Vector3(
            randomDirection.x * randomDistance,
            0f,
            randomDirection.y * randomDistance
        );

        Vector3 spawnPosition = playerTransform.position + spawnOffset;

        float minPosition = -mapHalfSize + spawnBoundaryMargin;
        float maxPosition = mapHalfSize - spawnBoundaryMargin;

        spawnPosition.x = Mathf.Clamp(spawnPosition.x, minPosition, maxPosition);
        spawnPosition.z = Mathf.Clamp(spawnPosition.z, minPosition, maxPosition);
        spawnPosition.y = spawnY;

        return spawnPosition;
    }
}