using UnityEngine;

public class Nuke : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        Inventory inventory = other.GetComponent<Inventory>();
        SoundEffectPlayer soundEffectPlayer = FindAnyObjectByType<SoundEffectPlayer>();

        if (playerController != null)
        {
            inventory.AddItemToInventory("Nuke");
            soundEffectPlayer.PlaySound(SoundEffectType.ItemCollect);
            int nbItemsCollections = PlayerPrefs.GetInt("itemsCollected");
            PlayerPrefs.SetInt("itemsCollected", nbItemsCollections + 1);
            Destroy(gameObject);
        }
    }
}