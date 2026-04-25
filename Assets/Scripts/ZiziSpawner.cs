using System.Collections;
using UnityEngine;

public class ZiziSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _zizi;
    [SerializeField]
    private float _spawnInterval = 3f;
    private SoundEffectPlayer soundEffectPlayer; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();
        StartCoroutine(SpawnZizi(_spawnInterval, _zizi));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnZizi(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-25f, 25f), 1.44f, Random.Range(-25f, 25f)), Quaternion.identity);
        soundEffectPlayer.PlayZiziSpawnSound();
        StartCoroutine(SpawnZizi(interval, enemy));
    }
}
