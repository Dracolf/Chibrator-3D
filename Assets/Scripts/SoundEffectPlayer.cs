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
    Boom,
    SidaSpawn,
    NukeIncoming,
    Yamete,
    ItemCollect,
    ZiziHit,
    ZiziDeath,
    BossHit,
    SoftProut,
    Buy,
    Error
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
                if (soundType == SoundEffectType.ZiziHit || soundType == SoundEffectType.ZiziDeath)
                {
                    audioSource.PlayOneShot(soundEffect.clip, 0.5f);
                    return;
                }
                if (soundType == SoundEffectType.BossSpawn || soundType == SoundEffectType.BossDeath)
                {
                    audioSource.PlayOneShot(soundEffect.clip, 1.5f);
                    return;
                }
                if (soundType == SoundEffectType.Error)
                {
                    audioSource.PlayOneShot(soundEffect.clip, 2f);
                    return;
                }
                audioSource.PlayOneShot(soundEffect.clip);
                return;
            }
        }

        Debug.LogWarning("Aucun son trouvé pour : " + soundType);
    }
}