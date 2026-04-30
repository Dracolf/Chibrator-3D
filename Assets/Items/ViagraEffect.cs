using UnityEngine;

public class ViagraEffect : MonoBehaviour
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
                "Viagra",
                PlayerEffects.EffectType.DamageMultiplier,
                2f,
                15f
            );

            if (soundEffectPlayer != null)
            {
                soundEffectPlayer.PlayViagraSound();
            }

            Destroy(gameObject);
        }
    }
}
