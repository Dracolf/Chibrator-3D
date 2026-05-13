using UnityEngine;

public class AudioSourceManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    void Start()
    {
        if (PlayerPrefs.GetInt("isGameMute") == 1)
        {
            audioSource.Stop();
        } else
        {
            audioSource.Play();
        }
    }
}
