using UnityEngine;

public class MedikitEffect : MonoBehaviour
{
    private SoundEffectPlayer soundEffectPlayer;

    private void Awake()
    {
        soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerController.ChangeHealth(50f);

            if (soundEffectPlayer != null)
            {
                soundEffectPlayer.PlayMedikitSound();
            }

            Destroy(gameObject);
        }
    }
}
