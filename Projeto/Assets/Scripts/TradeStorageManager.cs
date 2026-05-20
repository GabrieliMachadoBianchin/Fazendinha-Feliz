using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// Gerencia o armazém do jogador.

public class TradeStorageManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject storageUI;
    public List<GameObject> slots = new List<GameObject>();

    [Header("Configuração")]
    public int maxCapacity = 20;

    // Dicionário interno: item -> quantidade armazenada
    private Dictionary<InventoryItem, int> storedItems = new Dictionary<InventoryItem, int>();

    private bool isOpen = false;

    public static TradeStorageManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (storageUI != null)
            storageUI.SetActive(false);

        RefreshUI();
    }


    public void ToggleStorage()
    {
        isOpen = !isOpen;
        storageUI.SetActive(isOpen);

        if (isOpen) RefreshUI();
    }

    public void CloseStorage()
    {
        isOpen = false;
        storageUI.SetActive(false);
    }

    public int TotalItems()
    {
        int total = 0;
        foreach (var kvp in storedItems) total += kvp.Value;
        return total;
    }

    public bool AddItem(InventoryItem item, int amount = 1)
    {
        if (TotalItems() + amount > maxCapacity)
        {
            Debug.LogWarning("Armazém cheio! Capacidade máxima: " + maxCapacity);
            return false;
        }

        if (storedItems.ContainsKey(item))
            storedItems[item] += amount;
        else
            storedItems[item] = amount;

        Debug.Log($"Armazenado: {amount}x {item.itemName}  (total no armazém: {TotalItems()}/{maxCapacity})");

        if (isOpen) RefreshUI();
        return true;
    }

    public bool RemoveItem(InventoryItem item, int amount = 1)
    {
        if (!storedItems.ContainsKey(item) || storedItems[item] < amount)
        {
            Debug.LogWarning($"Itens insuficientes no armazém: {item.itemName}");
            return false;
        }

        storedItems[item] -= amount;
        if (storedItems[item] <= 0)
            storedItems.Remove(item);

        Debug.Log($"Removido do armazém: {amount}x {item.itemName}");

        if (isOpen) RefreshUI();
        return true;
    }

    public int GetQuantity(InventoryItem item)
    {
        return storedItems.ContainsKey(item) ? storedItems[item] : 0;
    }


    void RefreshUI()
    {
        List<InventoryItem> keys = new List<InventoryItem>(storedItems.Keys);

        for (int i = 0; i < slots.Count; i++)
        {
            Image icon     = slots[i].GetComponentInChildren<Image>();
            TextMeshProUGUI qty = slots[i].GetComponentInChildren<TextMeshProUGUI>();

            if (i < keys.Count)
            {
                InventoryItem item = keys[i];
                if (icon != null) { icon.sprite = item.icon; icon.color = Color.white; }
                if (qty  != null)   qty.text = storedItems[item].ToString();
            }
            else
            {
                if (icon != null) { icon.sprite = null; icon.color = new Color(1,1,1,0); }
                if (qty  != null)   qty.text = "";
            }
        }
    }
}
