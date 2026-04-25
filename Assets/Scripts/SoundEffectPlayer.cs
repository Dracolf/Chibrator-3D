using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip throwSound, ziziSpawnSound;

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
}