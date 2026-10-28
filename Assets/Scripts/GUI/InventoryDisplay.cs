using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField]
    private Image inv1, inv2, inv3;

    [SerializeField]
    private Sprite emptyInv, nukeInv, pillowInv, leurreInv;

    [SerializeField]
    private TextMeshProUGUI count1, count2, count3;

    [SerializeField]
    private TextMeshProUGUI nukeCooldownText;

    private Inventory inventory;
    private PlayerInputController playerInputController;

    private void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();
        playerInputController = FindAnyObjectByType<PlayerInputController>();

        UpdateInventoryOnScreen();
    }

    private void Update()
    {
        UpdateInventoryOnScreen();
    }

    public void UpdateInventoryOnScreen()
    {
        if (inventory == null)
        {
            return;
        }

        UpdateNukeSlot();
        UpdateBodyPillowSlot();
        UpdateLeurreSlot();
    }

    private void UpdateNukeSlot()
    {
        bool hasNuke = inventory.HasItem("Nuke");

        if (hasNuke)
        {
            inv1.sprite = nukeInv;
            count1.gameObject.SetActive(true);
            count1.text = inventory.GetItemCount("Nuke").ToString();
        }
        else
        {
            inv1.sprite = emptyInv;
            count1.gameObject.SetActive(false);
        }

        if (nukeCooldownText == null || playerInputController == null)
        {
            return;
        }

        if (hasNuke && playerInputController.IsNukeOnCooldown)
        {
            nukeCooldownText.gameObject.SetActive(true);
            nukeCooldownText.text = Mathf.CeilToInt(playerInputController.NukeCooldownRemaining).ToString() + "s";
        }
        else
        {
            nukeCooldownText.gameObject.SetActive(false);
        }
    }

    private void UpdateBodyPillowSlot()
    {
        if (inventory.HasItem("BodyPillow"))
        {
            inv2.sprite = pillowInv;
            count2.gameObject.SetActive(true);
            count2.text = inventory.GetItemCount("BodyPillow").ToString();
        }
        else
        {
            inv2.sprite = emptyInv;
            count2.gameObject.SetActive(false);
        }
    }

    private void UpdateLeurreSlot()
    {
        if (inventory.HasItem("Leurre"))
        {
            inv3.sprite = leurreInv;
            count3.gameObject.SetActive(true);
            count3.text = inventory.GetItemCount("Leurre").ToString();
        }
        else
        {
            inv3.sprite = emptyInv;
            count3.gameObject.SetActive(false);
        }
    }
}