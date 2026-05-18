using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<InventoryItem> unlockedItems = new List<InventoryItem>();

    public List<Button> slots = new List<Button>();

    public int playerLevel = 1;

    public List<InventoryItem> allItems = new List<InventoryItem>();

    void Start()
    {
        UpdateInventory();
    }

    public void UpdateInventory()
    {
        unlockedItems.Clear();

        foreach (InventoryItem item in allItems)
        {
            if (playerLevel >= item.requiredLevel)
            {
                unlockedItems.Add(item);
            }
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < unlockedItems.Count)
            {
                slots[i].image.sprite = unlockedItems[i].icon;

                slots[i].image.color = Color.white;
            }
            else
            {
                slots[i].image.sprite = null;

                slots[i].image.color = new Color(1, 1, 1, 0);
            }
        }
    }
}