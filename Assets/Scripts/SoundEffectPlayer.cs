using System;
using UnityEngine;

public enum SoundEffectType
{
    Throw,
    ZiziSpawn,
    Lubrifiant,
    Viagra,
    Medikit,
    BossSpawn,
    BossDeath,
    TerroristeSpawn,
    TerroristeDeath
}

public class SoundEffectPlayer : MonoBehaviour
{
    [Serializable]
    private class SoundEffect
    {
        public SoundEffectType type;
        public AudioClip clip;
    }

    [SerializeField]
    private SoundEffect[] soundEffects;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundEffectType soundType)
    {
        foreach (SoundEffect soundEffect in soundEffects)
        {
            if (soundEffect.type == soundType)
            {
                audioSource.PlayOneShot(soundEffect.clip);
                return;
            }
        }

        Debug.LogWarning("Aucun son trouvé pour : " + soundType);
    }
}