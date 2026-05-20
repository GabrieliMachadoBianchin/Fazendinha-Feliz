using UnityEngine;
using TMPro;

/// NPC que compra cenouras do jogador em troca de moedas.
public class TradeNPC : MonoBehaviour
{
    [Header("Troca")]
    public int carrotsRequired = 1;   // cenouras necessárias por troca
    public int coinsRewarded   = 10;  // moedas dadas por troca

    [Header("UI")]
    public GameObject tradeUI;
    public TextMeshProUGUI feedbackText;

    [Header("Interação")]
    public float interactDistance = 3f;
    public Transform playerTransform;

    private bool isOpen = false;
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
    }

    void Update()
    {
        if (playerTransform == null) return;

        float dist = Vector3.Distance(transform.position, playerTransform.position);

        if (dist <= interactDistance && Input.GetKeyDown(KeyCode.E))
            ToggleTrade();

        if (isOpen && dist > interactDistance * 1.5f)
            CloseTrade();
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
        SetFeedback($"Vendo {carrotsRequired} cenoura(s) por {coinsRewarded} moedas!");
    }

    public void CloseTrade()
    {
        isOpen = false;
        if (tradeUI != null) tradeUI.SetActive(false);
    }

    /// Chame este método pelo botão "Vender" na UI do NPC.
    public void SellCarrots()
    {
        if (StorageManager.Instance == null)
        {
            SetFeedback("Armazém não encontrado!");
            return;
        }

        if (!StorageManager.Instance.RemoveCarrots(carrotsRequired))
        {
            SetFeedback($"Você precisa de {carrotsRequired} cenoura(s)!");
            return;
        }

        if (coinManager != null)
            coinManager.AddCoins(coinsRewarded);

        SetFeedback($"Vendido! +{coinsRewarded} moedas.");
        Debug.Log($"[NPC] Troca: {carrotsRequired} cenoura(s) → {coinsRewarded} moedas.");
    }

    void SetFeedback(string msg)
    {
        if (feedbackText != null)
            feedbackText.text = msg;
    }
}