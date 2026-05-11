using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidaSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _sida;

    [SerializeField]
    private float _spawnInterval = 30f;

    [SerializeField]
    private int sidaCountPerWave = 5;

    [SerializeField]
    private float spawnY = 50f;

    [Header("Rain")]
    [SerializeField]
    private float rainDuration = 4f;

    [Header("UI")]
    [SerializeField]
    private SidaRainCountdownUI sidaRainCountdownUI;

    [Header("Spawn Area")]
    [SerializeField]
    private float spawnMinX = -30f;

    [SerializeField]
    private float spawnMaxX = 30f;

    [SerializeField]
    private float spawnMinZ = -30f;

    [SerializeField]
    private float spawnMaxZ = 30f;

    [Header("Spacing")]
    [SerializeField]
    private float minDistanceBetweenSidas = 20f;

    [SerializeField]
    private int maxPositionAttempts = 30;

    private SoundEffectPlayer soundEffectPlayer;

    private void Start()
    {
        soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();

        if (sidaRainCountdownUI != null)
        {
            sidaRainCountdownUI.ShowCountdown(_spawnInterval);
        }

        StartCoroutine(SidaRainLoop());
    }

    private IEnumerator SidaRainLoop()
    {
        while (true)
        {
            yield return WaitBeforeNextRain();

            SpawnSidaWave();

            yield return RainInProgress();
        }
    }

    private IEnumerator WaitBeforeNextRain()
    {
        float remainingTime = _spawnInterval;

        while (remainingTime > 0f)
        {
            if (sidaRainCountdownUI != null)
            {
                sidaRainCountdownUI.ShowCountdown(remainingTime);
            }

            remainingTime -= Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator RainInProgress()
    {
        if (sidaRainCountdownUI != null)
        {
            sidaRainCountdownUI.HideCountdown();
        }

        yield return new WaitForSeconds(rainDuration);
    }

    private void SpawnSidaWave()
    {
        List<Vector3> spawnPositions = GenerateSpawnPositions();

        foreach (Vector3 spawnPosition in spawnPositions)
        {
            Instantiate(
                _sida,
                spawnPosition,
                Quaternion.identity
            );

            if (soundEffectPlayer != null)
            {
                soundEffectPlayer.PlaySound(SoundEffectType.SidaSpawn);
            }
        }
    }

    private List<Vector3> GenerateSpawnPositions()
    {
        List<Vector3> spawnPositions = new List<Vector3>();

        for (int i = 0; i < sidaCountPerWave; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition(spawnPositions);
            spawnPositions.Add(spawnPosition);
        }

        return spawnPositions;
    }

    private Vector3 GetValidSpawnPosition(List<Vector3> existingPositions)
    {
        for (int attempt = 0; attempt < maxPositionAttempts; attempt++)
        {
            Vector3 candidatePosition = new Vector3(
                Random.Range(spawnMinX, spawnMaxX),
                spawnY,
                Random.Range(spawnMinZ, spawnMaxZ)
            );

            if (IsFarEnoughFromExistingPositions(candidatePosition, existingPositions))
            {
                return candidatePosition;
            }
        }

        return new Vector3(
            Random.Range(spawnMinX, spawnMaxX),
            spawnY,
            Random.Range(spawnMinZ, spawnMaxZ)
        );
    }

    private bool IsFarEnoughFromExistingPositions(Vector3 candidatePosition, List<Vector3> existingPositions)
    {
        foreach (Vector3 existingPosition in existingPositions)
        {
            float distance = Vector2.Distance(
                new Vector2(candidatePosition.x, candidatePosition.z),
                new Vector2(existingPosition.x, existingPosition.z)
            );

            if (distance < minDistanceBetweenSidas)
            {
                return false;
            }
        }

        return true;
    }
}