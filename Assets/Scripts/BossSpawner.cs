using System.Collections;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _boss;
    private SoundEffectPlayer soundEffectPlayer; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();
    }

    public void SpawnBoss()
    {
        GameObject newBoss = Instantiate(_boss, new Vector3(Random.Range(-25f, 25f), 3.4f, Random.Range(-25f, 25f)), Quaternion.identity);
        soundEffectPlayer.PlaySound(SoundEffectType.BossSpawn);
    }
}
