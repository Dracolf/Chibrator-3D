using UnityEngine;

public class LubrifiantEffect : MonoBehaviour
{
    private SoundEffectPlayer soundEffectPlayer;

    private void Awake()
    {
        soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerEffects playerEffects = other.GetComponent<PlayerEffects>();

        if (playerEffects != null)
        {
            playerEffects.ApplyTimedEffect(
                "Lubrifiant",
                PlayerEffects.EffectType.SpeedMultiplier,
                2f,
                15f
            );

            if (soundEffectPlayer != null)
            {
                soundEffectPlayer.PlayLubrifiantSound();
            }

            Destroy(gameObject);
        }
    }
}