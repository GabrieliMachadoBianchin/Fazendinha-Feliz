using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemRowUI : MonoBehaviour
{
    public Image icon;

    public TextMeshProUGUI itemNameText;

    public TextMeshProUGUI quantityText;

    public Button useButton;

    public Button sellButton;

    private ItemData currentItem;

    private int currentQuantity;

    public void Setup(ItemData item, int quantity)
    {
        currentItem = item;
        currentQuantity = quantity;

        icon.sprite = item.icon;

        itemNameText.text = item.itemName;

        quantityText.text = "x" + currentQuantity;

        useButton.onClick.RemoveAllListeners();
        sellButton.onClick.RemoveAllListeners();

        useButton.onClick.AddListener(UseItem);
        sellButton.onClick.AddListener(SellItem);
    }

    void UseItem()
    {
        Debug.Log("Usou: " + currentItem.itemName);
    }

    void SellItem()
    {
        if (currentQuantity > 0)
        {
            currentQuantity--;

            quantityText.text = "x" + currentQuantity;

            StorageManager.Instance.RemoveItem(currentItem.itemName, 1);

            Debug.Log("Vendeu: " + currentItem.itemName);
        }
    }
}
/*using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemRowUI : MonoBehaviour
{
    public Image icon;

    public TextMeshProUGUI itemNameText;

    public TextMeshProUGUI quantityText;

    public Button useButton;

    public Button sellButton;

    private ItemData currentItem;

    public void Setup(ItemData item)
    {
        currentItem = item;

        icon.sprite = item.icon;

        itemNameText.text = item.itemName;

        quantityText.text = "x" + item.quantity;

        useButton.onClick.AddListener(UseItem);

        sellButton.onClick.AddListener(SellItem);
    }

    void UseItem()
    {
        Debug.Log("Usou: " + currentItem.itemName);
    }

    void SellItem()
    {
        if (currentItem.quantity > 0)
        {
            currentItem.quantity--;

            quantityText.text = "x" + currentItem.quantity;

            Debug.Log("Vendeu: " + currentItem.itemName);
        }
    }
}*/