using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [Header("Drop Rate")]
    [SerializeField]
    private float dropChance = 0.90f;

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

    [SerializeField]
    private GameObject leurre;

    [Header("Drop Settings")]
    [SerializeField]
    private float dropY = 1.44f;

    [SerializeField]
    private float itemLifetime = 120f;

    private PlayerController playerController;
    private RastaSpawner rastaSpawner;
    private ChinoisSpawner chinoisSpawner;
    private Score score;

    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        rastaSpawner = FindAnyObjectByType<RastaSpawner>();
        chinoisSpawner = FindAnyObjectByType<ChinoisSpawner>();
        score = FindAnyObjectByType<Score>();
    }

    public void TryDropBonus()
    {
        if (score.isScoringEnabled)
        {
            if (Random.value > dropChance)
            {
                if(Random.Range(0,2) == 0)
                {
                    rastaSpawner.SpawnRasta();
                    return; 
                }
                chinoisSpawner.SpawnChinois();
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
        int randomWeight = Random.Range(0, 13);

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

        if (randomWeight < 11)
        {
            return leurre;
        }

        if (randomWeight < 12)
        {
            return nuke;
        }

        return bodyPillow;
    }

    private GameObject GetRandomItemWithoutMedikit()
    {
        int randomWeight = Random.Range(0, 10);

        if (randomWeight < 3)
        {
            return viagra;
        }

        if (randomWeight < 6)
        {
            return lubrifiant;
        }

        if (randomWeight < 8)
        {
            return leurre;
        }

        if (randomWeight < 9)
        {
            return nuke;
        }

        return bodyPillow;
    }
}