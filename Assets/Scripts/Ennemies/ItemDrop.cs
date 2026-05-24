using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [Header("Drop Rate")]
    [SerializeField]
    private float dropChance = 0.85f;

    [Header("Common Items")]
    [SerializeField]
    private GameObject viagra;

    [SerializeField]
    private GameObject lubrifiant;

    [SerializeField]
    private GameObject medikit;

    [Header("Rare Items")]
    [SerializeField]
    private GameObject nuke;

    [SerializeField]
    private GameObject bodyPillow;

    [Header("Drop Settings")]
    [SerializeField]
    private float dropY = 1.44f;

    [SerializeField]
    private float itemLifetime = 120f;

    private PlayerController playerController;
    private RastaSpawner rastaSpawner;

    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        rastaSpawner = FindAnyObjectByType<RastaSpawner>();
    }

    public void TryDropBonus()
    {
        if (Random.value > dropChance)
        {
            rastaSpawner.SpawnRasta();
            return;
        }

        GameObject itemToDrop = GetRandomItemToDrop();

        if (itemToDrop == null)
        {
            return;
        }

        Vector3 dropPosition = new Vector3(
            transform.position.x,
            dropY,
            transform.position.z
        );

        GameObject droppedItem = Instantiate(
            itemToDrop,
            dropPosition,
            Quaternion.identity
        );

        Destroy(droppedItem, itemLifetime);
    }

    private GameObject GetRandomItemToDrop()
    {
        bool playerNeedsHealth =
            playerController != null &&
            playerController.currentHealth < playerController.maxHealth;

        if (playerNeedsHealth)
        {
            return GetRandomItemWithMedikit();
        }

        return GetRandomItemWithoutMedikit();
    }

    private GameObject GetRandomItemWithMedikit()
    {
        int randomWeight = Random.Range(0, 11);

        if (randomWeight < 3)
        {
            return medikit;
        }

        if (randomWeight < 6)
        {
            return viagra;
        }

        if (randomWeight < 9)
        {
            return lubrifiant;
        }

        if (randomWeight < 10)
        {
            return nuke;
        }

        return bodyPillow;
    }

    private GameObject GetRandomItemWithoutMedikit()
    {
        int randomWeight = Random.Range(0, 8);

        if (randomWeight < 3)
        {
            return viagra;
        }

        if (randomWeight < 6)
        {
            return lubrifiant;
        }

        if (randomWeight < 7)
        {
            return nuke;
        }

        return bodyPillow;
    }
}