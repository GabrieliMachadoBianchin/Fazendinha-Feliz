using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ShopOffer
{
    public string itemId;
    public string itemName;
    public int priceInCoins;
    public int quantityAvailable;
    public bool isBuyFromPlayer; // true = NPC compra do jogador; false = jogador compra do NPC
}

public class NPCTrader : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcName = "Dona Celeste";
    public string greeting = "Olá! Posso comprar sua colheita ou te vender sementes!";

    [Header("Ofertas de Compra (NPC compra do jogador)")]
    public List<ShopOffer> buyOffers = new List<ShopOffer>();

    [Header("Ofertas de Venda (Jogador compra do NPC)")]
    public List<ShopOffer> sellOffers = new List<ShopOffer>();

    [Header("Animation")]
    public Animator animator;

    [Header("Interaction Range")]
    public float range = 3f;

    private bool playerInRange;

    void Update()
    {
        animator?.SetBool("PlayerNearby", playerInRange);
    }

    public void OpenShop()
    {
        UIManager.Instance?.OpenShopPanel(this);
    }

    public bool BuyFromPlayer(string itemId, int qty)
    {
        var offer = buyOffers.Find(o => o.itemId == itemId);
        if (offer == null) return false;

        if (!GameManager.Instance.RemoveFromInventory(itemId, qty)) return false;

        int total = offer.priceInCoins * qty;
        GameManager.Instance.AddCoins(total);
        GameManager.Instance.OnSell?.Invoke(total);

        UIManager.Instance?.ShowNotification($"Vendeu {qty}x {offer.itemName} por {total} moedas!");
        return true;
    }

    public bool SellToPlayer(string itemId, int qty)
    {
        var offer = sellOffers.Find(o => o.itemId == itemId);
        if (offer == null) return false;

        if (offer.quantityAvailable >= 0 && offer.quantityAvailable < qty) return false;

        int total = offer.priceInCoins * qty;
        if (!GameManager.Instance.SpendCoins(total)) return false;
        GameManager.Instance.OnBuy?.Invoke(total);

        if (offer.quantityAvailable > 0) offer.quantityAvailable -= qty;

        GameManager.Instance.AddToInventory(itemId, offer.itemName, qty, offer.priceInCoins, false);
        UIManager.Instance?.ShowNotification($"Comprou {qty}x {offer.itemName} por {total} moedas!");
        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerInRange = false;
    }

    public bool IsPlayerInRange() => playerInRange;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
