using System.Collections;
using UnityEngine;

public class ZiziSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _zizi;

    [SerializeField]
    private float _spawnInterval = 3f;

    [SerializeField]
    private bool canCreateNewSpawners = true;

    [Header("Spawn Around Player")]
    [SerializeField]
    private float minSpawnDistanceFromPlayer = 8f;

    [SerializeField]
    private float maxSpawnDistanceFromPlayer = 12f;

    [SerializeField]
    private float spawnY = 1.4f;

    [Header("Map Limits")]
    [SerializeField]
    private float mapHalfSize = 37.5f;

    [SerializeField]
    private float spawnBoundaryMargin = 1.5f;

    private SoundEffectPlayer soundEffectPlayer;
    private Score score;
    private Transform playerTransform;

    private int lastSpawnerScoreThreshold = 0;

    private void Start()
    {
        soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();
        score = FindAnyObjectByType<Score>();

        PlayerController playerController = FindAnyObjectByType<PlayerController>();

        if (playerController != null)
        {
            playerTransform = playerController.transform;
        }

        StartCoroutine(SpawnZizi(_spawnInterval, _zizi));
    }

    private IEnumerator SpawnZizi(float interval, GameObject enemy)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            Vector3 spawnPosition = GetSpawnPositionAroundPlayer();

            Instantiate(
                enemy,
                spawnPosition,
                Quaternion.identity
            );

            if (soundEffectPlayer != null)
            {
                soundEffectPlayer.PlaySound(SoundEffectType.ZiziSpawn);
            }

            TryCreateSpawner();
        }
    }

    private Vector3 GetSpawnPositionAroundPlayer()
    {
        return EnemySpawnPositionHelper.GetSpawnPositionAroundPlayer(
            playerTransform,
            minSpawnDistanceFromPlayer,
            maxSpawnDistanceFromPlayer,
            spawnY,
            mapHalfSize,
            spawnBoundaryMargin
        );
    }

    private void TryCreateSpawner()
    {
        if (!canCreateNewSpawners || score == null)
        {
            return;
        }

        int currentSpawnerScoreThreshold = score.score / 1000;

        if (currentSpawnerScoreThreshold > lastSpawnerScoreThreshold)
        {
            lastSpawnerScoreThreshold = currentSpawnerScoreThreshold;

            GameObject newSpawnerObj = Instantiate(
                gameObject,
                transform.position,
                Quaternion.identity
            );

            ZiziSpawner newSpawner = newSpawnerObj.GetComponent<ZiziSpawner>();

            if (newSpawner != null)
            {
                newSpawner.canCreateNewSpawners = false;
                newSpawner._spawnInterval *= 3;
            }
        }
    }
}