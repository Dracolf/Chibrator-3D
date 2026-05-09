using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private readonly Dictionary<string, int> inventory = new();

    private void Start()
    {
        inventory["Nuke"] = PlayerPrefs.GetInt("nukeAmount");
        inventory["BodyPillow"] = PlayerPrefs.GetInt("pillowAmount");
    }

    public void AddItemToInventory(string item)
    {
        if (!inventory.ContainsKey(item))
        {
            inventory[item] = 1;
        }
        else
        {
            inventory[item]++;
        }
    }

    public void RemoveItemFromInventory(string item)
    {
        if (!inventory.ContainsKey(item))
        {
            return;
        }

        inventory[item]--;

        if (inventory[item] <= 0)
        {
            inventory.Remove(item);
        }
    }

    public bool HasItem(string item)
    {
        return inventory.ContainsKey(item) && inventory[item] > 0;
    }

    public int GetItemCount(string item)
    {
        if (!inventory.ContainsKey(item))
        {
            return 0;
        }
        return inventory[item];
    }
}
