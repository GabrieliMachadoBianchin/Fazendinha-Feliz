using System;

[Serializable]
public class InventoryItem
{
    public string itemId;
    public string itemName;
    public int quantity;
    public int unitValue;
    public bool isCrop;

    public InventoryItem(string id, string name, int qty, int value, bool crop = true)
    {
        itemId = id;
        itemName = name;
        quantity = qty;
        unitValue = value;
        isCrop = crop;
    }
}
