using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip throwSound, ziziSpawnSound, lubrifiantSound, viagraSound,
        medikitSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayThrowSound()
    {
        audioSource.PlayOneShot(throwSound);
    }

    public void PlayZiziSpawnSound()
    {
        audioSource.PlayOneShot(ziziSpawnSound);
    }

    public void PlayLubrifiantSound()
    {
        audioSource.PlayOneShot(lubrifiantSound);
    }

    public void PlayViagraSound()
    {
        audioSource.PlayOneShot(viagraSound);
    }

    public void PlayMedikitSound()
    {
        audioSource.PlayOneShot(medikitSound);
    }
}