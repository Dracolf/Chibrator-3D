using System.Collections;
using UnityEngine;

public class TerroristeSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _terroriste;
    private SoundEffectPlayer soundEffectPlayer; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();
    }

    public void SpawnTerroriste()
    {
        GameObject newTerroriste = Instantiate(_terroriste, new Vector3(Random.Range(-25f, 25f), 1.4f, Random.Range(-25f, 25f)), Quaternion.identity);
        soundEffectPlayer.PlaySound(SoundEffectType.TerroristeSpawn);
    }
}
