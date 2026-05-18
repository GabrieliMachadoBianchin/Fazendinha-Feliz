using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Configurações Iniciais")]
    public int startingCoins = 50;

    [Header("Crops Disponíveis")]
    public List<CropData> availableCrops = new List<CropData>();

    // Estado do jogo
    private int coins;
    private List<InventoryItem> inventory = new List<InventoryItem>();

    // Eventos
    public System.Action<int> OnCoinsChanged;
    public System.Action<List<InventoryItem>> OnInventoryChanged;

    // Eventos para o LevelSystem
    public System.Action<string> OnHarvest;   // cropName
    public System.Action<int> OnSell;         // moedas ganhas
    public System.Action<int> OnBuy;          // moedas gastas

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        InitGame();
    }

    void InitGame()
    {
        coins = startingCoins;
        inventory.Clear();
    }

    // === MOEDAS ===
    public int GetCoins() => coins;

    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            OnCoinsChanged?.Invoke(coins);
            return true;
        }
        return false;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        OnCoinsChanged?.Invoke(coins);
    }

    // === INVENTÁRIO ===
    public List<InventoryItem> GetInventory() => inventory;

    public void AddToInventory(string itemId, string itemName, int qty, int unitValue, bool isCrop = true)
    {
        var existing = inventory.Find(i => i.itemId == itemId);
        if (existing != null)
        {
            existing.quantity += qty;
        }
        else
        {
            inventory.Add(new InventoryItem(itemId, itemName, qty, unitValue, isCrop));
        }
        OnInventoryChanged?.Invoke(inventory);

        if (isCrop)
            OnHarvest?.Invoke(itemName);
    }

    public bool RemoveFromInventory(string itemId, int qty)
    {
        var item = inventory.Find(i => i.itemId == itemId);
        if (item != null && item.quantity >= qty)
        {
            item.quantity -= qty;
            if (item.quantity <= 0)
                inventory.Remove(item);
            OnInventoryChanged?.Invoke(inventory);
            return true;
        }
        return false;
    }

    public int GetItemCount(string itemId)
    {
        var item = inventory.Find(i => i.itemId == itemId);
        return item?.quantity ?? 0;
    }

    public CropData GetCropData(string cropId)
    {
        return availableCrops.Find(c => c.name == cropId);
    }
}
