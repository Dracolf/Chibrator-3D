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
    Error,
    OneLove,
    Bomboclaat
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
        audioSource.ignoreListenerPause = true;
    }

    public void PlaySound(SoundEffectType soundType)
    {
        AudioClip clip = GetClip(soundType);

        if (clip == null)
        {
            Debug.LogWarning("Aucun son trouvé pour : " + soundType);
            return;
        }

        float volume = GetVolume(soundType);

        audioSource.PlayOneShot(clip, volume);
    }

    public void PlaySoundPersistAfterSceneLoad(SoundEffectType soundType)
    {
        AudioClip clip = GetClip(soundType);

        if (clip == null)
        {
            Debug.LogWarning("Aucun son trouvé pour : " + soundType);
            return;
        }

        float volume = GetVolume(soundType);

        GameObject audioObject = new GameObject("Persistent One Shot Audio - " + soundType);
        DontDestroyOnLoad(audioObject);

        AudioSource persistentAudioSource = audioObject.AddComponent<AudioSource>();
        persistentAudioSource.clip = clip;
        persistentAudioSource.volume = volume;
        persistentAudioSource.spatialBlend = 0f;
        persistentAudioSource.playOnAwake = false;

        persistentAudioSource.Play();

        Destroy(audioObject, clip.length + 0.1f);
    }

    private AudioClip GetClip(SoundEffectType soundType)
    {
        foreach (SoundEffect soundEffect in soundEffects)
        {
            if (soundEffect.type == soundType)
            {
                return soundEffect.clip;
            }
        }

        return null;
    }

    private float GetVolume(SoundEffectType soundType)
    {
        if (soundType == SoundEffectType.ZiziHit || soundType == SoundEffectType.ZiziDeath)
        {
            return 0.5f;
        }

        if (soundType == SoundEffectType.BossSpawn || soundType == SoundEffectType.BossDeath)
        {
            return 1.5f;
        }

        if (soundType == SoundEffectType.Error || soundType == SoundEffectType.OneLove)
        {
            return 2f;
        }

        return 1f;
    }
}