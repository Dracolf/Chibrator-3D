using UnityEngine;

public class TerroristeSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _terroriste;

    [Header("Spawn Around Player")]
    [SerializeField]
    private float minSpawnDistanceFromPlayer = 14f;

    [SerializeField]
    private float maxSpawnDistanceFromPlayer = 20f;

    [SerializeField]
    private float spawnY = 1.4f;

    [Header("Map Limits")]
    [SerializeField]
    private float mapHalfSize = 37.5f;

    [SerializeField]
    private float spawnBoundaryMargin = 1.5f;

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

    public void SpawnTerroriste()
    {
        Vector3 spawnPosition = GetSpawnPositionAroundPlayer();

        Instantiate(
            _terroriste,
            spawnPosition,
            Quaternion.identity
        );

        if (soundEffectPlayer != null)
        {
            soundEffectPlayer.PlaySound(SoundEffectType.TerroristeSpawn);
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
}