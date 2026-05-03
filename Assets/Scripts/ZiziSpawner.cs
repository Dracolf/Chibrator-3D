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

    private SoundEffectPlayer soundEffectPlayer;
    private Score score;

    private int lastSpawnerScoreThreshold = 0;

    private void Start()
    {
        soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();
        score = FindAnyObjectByType<Score>();

        StartCoroutine(SpawnZizi(_spawnInterval, _zizi));
    }

    private IEnumerator SpawnZizi(float interval, GameObject enemy)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            Instantiate(
                enemy,
                new Vector3(Random.Range(-25f, 25f), 1.4f, Random.Range(-25f, 25f)),
                Quaternion.identity
            );

            if (soundEffectPlayer != null)
            {
                soundEffectPlayer.PlaySound(SoundEffectType.ZiziSpawn);
            }

            TryCreateSpawner();
        }
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