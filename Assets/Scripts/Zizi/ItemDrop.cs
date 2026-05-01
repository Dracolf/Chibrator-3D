using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField]
    private GameObject viagra, lubrifiant, medikit;
    private PlayerController playerController;
    private GameObject[] bonusList;

    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        bonusList = new GameObject[] { medikit, viagra, lubrifiant };
    }

    public void DropBonus()
    {
        int randomItem;
        if (playerController.currentHealth < playerController.maxHealth)
        {
            randomItem = Random.Range(0,3);
        } else
        {
            randomItem = Random.Range(1,3);
        }

        Instantiate(bonusList[randomItem], new Vector3(transform.position.x, 1.44f, transform.position.z), Quaternion.identity);
        Destroy(gameObject, 120f);
    }
}
