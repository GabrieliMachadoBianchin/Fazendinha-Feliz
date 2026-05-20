using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TradeNPC : MonoBehaviour
{

    [System.Serializable]
    public class TradeOffer
    {
        [Header("O que o jogador entrega")]
        public InventoryItem itemRequerido;
        public int quantidadeRequerida = 1;

        [Header("O que o jogador recebe")]
        public bool recebeCoins = true;          // true  → recebe moedas
        public int  coinsRecompensa  = 10;       // usado quando recebeCoins = true
        public InventoryItem itemRecompensa;     // usado quando recebeCoins = false
        public int  quantidadeRecompensa = 1;

        [HideInInspector] public string Descricao =>
            $"{quantidadeRequerida}x {itemRequerido?.itemName ?? "?"} → " +
            (recebeCoins
                ? $"{coinsRecompensa} moedas"
                : $"{quantidadeRecompensa}x {itemRecompensa?.itemName ?? "?"}");
    }


    [Header("Ofertas")]
    public List<TradeOffer> tradeOffers = new List<TradeOffer>();

    [Header("UI")]
    public GameObject tradeUI;
    public Transform  offerContainer;
    public GameObject offerButtonPrefab;
    public TextMeshProUGUI feedbackText;

    [Header("Interação")]
    public float interactDistance = 3f;
    public Transform playerTransform;


    private bool isOpen      = false;
    private CoinManager coinManager;

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null) playerTransform = p.transform;
        }

        coinManager = FindObjectOfType<CoinManager>();

        if (tradeUI != null) tradeUI.SetActive(false);

        BuildOfferButtons();
    }

    void Update()
    {
        if (playerTransform == null) return;

        float dist = Vector3.Distance(transform.position, playerTransform.position);

        if (dist <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            ToggleTrade();
        }

        // Fecha automaticamente se o jogador se afastar
        if (isOpen && dist > interactDistance * 1.5f)
        {
            CloseTrade();
        }
    }

    public void ToggleTrade()
    {
        if (isOpen) CloseTrade();
        else        OpenTrade();
    }

    public void OpenTrade()
    {
        isOpen = true;
        if (tradeUI != null) tradeUI.SetActive(true);
        SetFeedback("");
        Debug.Log("NPC: Oi! O que quer trocar?");
    }

    public void CloseTrade()
    {
        isOpen = false;
        if (tradeUI != null) tradeUI.SetActive(false);
    }

    void BuildOfferButtons()
    {
        if (offerContainer == null || offerButtonPrefab == null) return;

        // Limpa botões antigos
        foreach (Transform child in offerContainer)
            Destroy(child.gameObject);

        for (int i = 0; i < tradeOffers.Count; i++)
        {
            int index = i; // captura para closure
            TradeOffer offer = tradeOffers[i];

            GameObject btn = Instantiate(offerButtonPrefab, offerContainer);

            TextMeshProUGUI[] texts = btn.GetComponentsInChildren<TextMeshProUGUI>();
            if (texts.Length >= 1) texts[0].text = offer.Descricao;

            
            Button button = btn.GetComponent<Button>();
            if (button != null)
                button.onClick.AddListener(() => ExecuteTrade(index));
        }
    }


    public void ExecuteTrade(int offerIndex)
    {
        if (offerIndex < 0 || offerIndex >= tradeOffers.Count) return;

        TradeOffer offer = tradeOffers[offerIndex];

        if (TradeStorageManager.Instance == null)
        {
            SetFeedback("Armazém não encontrado!");
            return;
        }

        // Verifica se o jogador tem os itens necessários
        if (TradeStorageManager.Instance.GetQuantity(offer.itemRequerido) < offer.quantidadeRequerida)
        {
            SetFeedback($"Você precisa de {offer.quantidadeRequerida}x {offer.itemRequerido.itemName}!");
            Debug.LogWarning("Troca recusada: itens insuficientes.");
            return;
        }

        // Remove do armazém
        bool removido = TradeStorageManager.Instance.RemoveItem(offer.itemRequerido, offer.quantidadeRequerida);
        if (!removido)
        {
            SetFeedback("Erro ao remover item do armazém.");
            return;
        }

        // Entrega a recompensa
        if (offer.recebeCoins)
        {
            if (coinManager != null)
                coinManager.AddCoins(offer.coinsRecompensa);

            SetFeedback($"Troca feita! +{offer.coinsRecompensa} moedas.");
            Debug.Log($"Troca: {offer.quantidadeRequerida}x {offer.itemRequerido.itemName} → {offer.coinsRecompensa} moedas.");
        }
        else
        {
            if (offer.itemRecompensa != null)
                TradeStorageManager.Instance.AddItem(offer.itemRecompensa, offer.quantidadeRecompensa);

            SetFeedback($"Troca feita! +{offer.quantidadeRecompensa}x {offer.itemRecompensa?.itemName}.");
            Debug.Log($"Troca: {offer.quantidadeRequerida}x {offer.itemRequerido.itemName} → {offer.quantidadeRecompensa}x {offer.itemRecompensa?.itemName}.");
        }
    }


    void SetFeedback(string msg)
    {
        if (feedbackText != null)
            feedbackText.text = msg;
    }
}

/*using UnityEngine;

public class TradeNPC : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}*/
