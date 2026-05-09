using UnityEngine;

public class BodyPillow : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        Inventory inventory = other.GetComponent<Inventory>();
        SoundEffectPlayer soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();

        if (playerController != null)
        {
            inventory.AddItemToInventory("BodyPillow");
            soundEffectPlayer.PlaySound(SoundEffectType.ItemCollect);
            Destroy(gameObject);
        }
    }
}
