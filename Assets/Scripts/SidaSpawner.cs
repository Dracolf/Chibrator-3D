using System.Collections;
using UnityEngine;

public class SidaSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _sida;

    [SerializeField]
    private float _spawnInterval = 60f;

    private SoundEffectPlayer soundEffectPlayer;

    private void Start()
    {
        soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();

        StartCoroutine(SpawnSidas(_spawnInterval, _sida));
    }

    private IEnumerator SpawnSidas(float interval, GameObject sida)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            for (int i = 0; i < 5; i++)
            {
                Instantiate(
                    sida,
                    new Vector3(Random.Range(-25f, 25f), 50f, Random.Range(-25f, 25f)),
                    Quaternion.identity
                );

                if (soundEffectPlayer != null)
                {
                    soundEffectPlayer.PlaySound(SoundEffectType.SidaSpawn);
                }
            }
        }
    }
}