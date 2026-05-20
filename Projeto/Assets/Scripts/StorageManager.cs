using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance;

    public GameObject storageUI;
    public Transform content;
    public GameObject itemRowTemplate;
    public Button closeButton;

    public List<ItemData> itemDatabase = new List<ItemData>();
    private Dictionary<string, int> inventory = new Dictionary<string, int>();

    private bool isOpen = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseStorage);
        }

        storageUI.SetActive(false);
        isOpen = false;
    }

    public void OpenStorage()
    {
        if (isOpen) return;

        isOpen = true;
        storageUI.SetActive(true);
        RefreshUI();
    }

    public void CloseStorage()
    {
        if (!isOpen) return;

        isOpen = false;
        storageUI.SetActive(false);
    }

    public bool IsOpen()
    {
        return isOpen;
    }

    public void AddItem(string itemName, int amount)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] += amount;
        }
        else
        {
            inventory.Add(itemName, amount);
        }

        RefreshUI();
    }

    public void RemoveItem(string itemName, int amount)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] -= amount;

            if (inventory[itemName] < 0)
            {
                inventory[itemName] = 0;
            }
        }

        RefreshUI();
    }

    public int GetQuantity(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            return inventory[itemName];
        }
        return 0;
    }

    public void RefreshUI()
    {
        foreach (Transform child in content)
        {
            if (child.gameObject == itemRowTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (ItemData item in itemDatabase)
        {
            int quantity = GetQuantity(item.itemName);

            GameObject row = Instantiate(itemRowTemplate, content);
            row.SetActive(true);

            ItemRowUI ui = row.GetComponent<ItemRowUI>();
            ui.Setup(item, quantity);
        }
    }

    public void SellItem(ItemData item)
    {
        if (GetQuantity(item.itemName) > 0)
        {
            RemoveItem(item.itemName, 1);
            Debug.Log("Vendeu: " + item.itemName);
        }
    }

    public void UseItem(ItemData item)
    {
        if (GetQuantity(item.itemName) > 0)
        {
            RemoveItem(item.itemName, 1);
            Debug.Log("Usou: " + item.itemName);
        }
    }
}

/*using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance;

    public GameObject storageUI;

    public Transform content;

    public GameObject itemRowTemplate;

    public List<ItemData> itemDatabase = new List<ItemData>();

    private Dictionary<string, int> inventory =
        new Dictionary<string, int>();

    private bool isOpen = false;

    public bool IsOpen()
{
    return isOpen;
}

public void OpenStorage()
{
    isOpen = true;

    storageUI.SetActive(true);

    RefreshUI();
}

public void CloseStorage()
{
    isOpen = false;

    storageUI.SetActive(false);
}
    void Awake()
    {
        Instance = this;
    }

public void ToggleStorage()
{
    isOpen = !isOpen;

    storageUI.SetActive(isOpen);

    Debug.Log("Storage aberto? " + isOpen);
}
    void Start()
{
    storageUI.SetActive(false);

    RefreshUI();
}
    public void AddItem(string itemName, int amount)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] += amount;
        }
        else
        {
            inventory.Add(itemName, amount);
        }

        RefreshUI();
    }

    public void RemoveItem(string itemName, int amount)
    {
        if (inventory.ContainsKey(itemName))
        {
            inventory[itemName] -= amount;

            if (inventory[itemName] <= 0)
            {
                inventory.Remove(itemName);
            }
        }

        RefreshUI();
    }

    public int GetQuantity(string itemName)
    {
        if (inventory.ContainsKey(itemName))
        {
            return inventory[itemName];
        }

        return 0;
    }

    public void RefreshUI()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (ItemData item in itemDatabase)
        {
            int quantity = GetQuantity(item.itemName);

            if (quantity > 0)
            {
                GameObject row =
                    Instantiate(itemRowTemplate, content);

                ItemRowUI ui =
                    row.GetComponent<ItemRowUI>();

                ui.Setup(item, quantity);
            }
        }
    }
}*/