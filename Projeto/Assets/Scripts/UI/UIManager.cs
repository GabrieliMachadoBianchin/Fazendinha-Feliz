using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("HUD")]
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI notificationText;
    public GameObject interactionHint;
    public TextMeshProUGUI interactionHintText;

    [Header("Inventário")]
    public GameObject inventoryPanel;
    public Transform inventoryGrid;
    public GameObject inventoryItemPrefab;
    public Button closeInventoryBtn;

    [Header("Plantio")]
    public GameObject plantingPanel;
    public Transform cropGrid;
    public GameObject cropButtonPrefab;
    public Button closePlantingBtn;
    public TextMeshProUGUI plantingTitle;

    [Header("Loja NPC")]
    public GameObject shopPanel;
    public TextMeshProUGUI shopTitle;
    public TextMeshProUGUI shopGreeting;
    public Transform buyOffersGrid;
    public Transform sellOffersGrid;
    public GameObject shopItemPrefab;
    public Button closeShopBtn;

    [Header("Plot Info")]
    public GameObject plotInfoPanel;
    public TextMeshProUGUI plotCropName;
    public Slider plotGrowthSlider;

    private FarmPlot selectedPlot;
    private NPCTrader currentTrader;
    private PlayerController player;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        GameManager.Instance.OnCoinsChanged += UpdateCoinsUI;
        GameManager.Instance.OnInventoryChanged += _ => RefreshInventoryIfOpen();
        UpdateCoinsUI(GameManager.Instance.GetCoins());

        closeInventoryBtn?.onClick.AddListener(CloseInventory);
        closePlantingBtn?.onClick.AddListener(ClosePlantingMenu);
        closeShopBtn?.onClick.AddListener(CloseShop);

        HideAll();
    }

    void Update()
    {
        // Tecla I = inventário
        if (Input.GetKeyDown(KeyCode.I))
            ToggleInventory();

        // Tecla Escape = fecha tudo
        if (Input.GetKeyDown(KeyCode.Escape))
            CloseAll();

        // Atualiza progresso do plot próximo
        UpdatePlotInfo();
    }

    void HideAll()
    {
        inventoryPanel?.SetActive(false);
        plantingPanel?.SetActive(false);
        shopPanel?.SetActive(false);
        plotInfoPanel?.SetActive(false);
        interactionHint?.SetActive(false);
        notificationText?.gameObject.SetActive(false);
    }

    // === HUD ===
    void UpdateCoinsUI(int coins)
    {
        if (coinsText) coinsText.text = $"🌾 {coins} moedas";
    }

    public void ShowNotification(string msg, float duration = 2.5f)
    {
        StopAllCoroutines();
        if (notificationText)
        {
            notificationText.text = msg;
            notificationText.gameObject.SetActive(true);
            StartCoroutine(HideNotification(duration));
        }
    }

    IEnumerator HideNotification(float t)
    {
        yield return new WaitForSeconds(t);
        notificationText?.gameObject.SetActive(false);
    }

    public void UpdateInteractionHint(FarmPlot plot, NPCTrader npc)
    {
        if (interactionHint == null) return;

        if (npc != null)
        {
            interactionHint.SetActive(true);
            interactionHintText.text = "[E] Falar com " + npc.npcName;
        }
        else if (plot != null)
        {
            interactionHint.SetActive(true);
            string action = plot.State == PlotState.Ready ? "Colher" :
                            plot.State == PlotState.Empty ? "Plantar" : "Crescendo...";
            interactionHintText.text = plot.State == PlotState.Growing ? action : $"[E] {action}";
        }
        else
        {
            interactionHint.SetActive(false);
        }
    }

    void UpdatePlotInfo()
    {
        // Encontra plot mais próximo do jogador
        var plots = FarmManager.Instance?.Plots;
        if (plots == null) return;

        FarmPlot nearest = null;
        float minD = float.MaxValue;
        foreach (var p in plots)
        {
            if (p.State == PlotState.Growing || p.State == PlotState.Planted)
            {
                float d = Vector3.Distance(player.transform.position, p.transform.position);
                if (d < 3f && d < minD) { minD = d; nearest = p; }
            }
        }

        if (nearest != null && plotInfoPanel != null)
        {
            plotInfoPanel.SetActive(true);
            plotCropName.text = nearest.CurrentCrop?.cropName ?? "";
            plotGrowthSlider.value = nearest.GetGrowthProgress();
        }
        else
        {
            plotInfoPanel?.SetActive(false);
        }
    }

    // === INVENTÁRIO ===
    void ToggleInventory()
    {
        if (inventoryPanel.activeSelf) CloseInventory();
        else OpenInventory();
    }

    public void OpenInventory()
    {
        CloseAll();
        inventoryPanel?.SetActive(true);
        player?.SetInteracting(true);
        RefreshInventory();
    }

    void CloseInventory()
    {
        inventoryPanel?.SetActive(false);
        player?.SetInteracting(false);
    }

    void RefreshInventoryIfOpen()
    {
        if (inventoryPanel != null && inventoryPanel.activeSelf)
            RefreshInventory();
    }

    void RefreshInventory()
    {
        if (inventoryGrid == null) return;
        foreach (Transform t in inventoryGrid) Destroy(t.gameObject);

        var items = GameManager.Instance.GetInventory();
        if (items.Count == 0)
        {
            var empty = new GameObject("EmptyLabel");
            empty.transform.SetParent(inventoryGrid, false);
            var tmp = empty.AddComponent<TextMeshProUGUI>();
            tmp.text = "Inventário vazio";
            tmp.color = Color.gray;
            tmp.fontSize = 16;
            return;
        }

        foreach (var item in items)
        {
            var go = Instantiate(inventoryItemPrefab, inventoryGrid);
            var nameLabel = go.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
            var qtyLabel = go.transform.Find("QtyText")?.GetComponent<TextMeshProUGUI>();
            var valLabel = go.transform.Find("ValText")?.GetComponent<TextMeshProUGUI>();

            if (nameLabel) nameLabel.text = item.itemName;
            if (qtyLabel) qtyLabel.text = $"x{item.quantity}";
            if (valLabel) valLabel.text = $"{item.unitValue * item.quantity} moedas";
        }
    }

    // === PLANTIO ===
    public void OpenPlantingMenu(FarmPlot plot)
    {
        if (plot.State != PlotState.Empty) return;
        selectedPlot = plot;
        CloseAll();
        plantingPanel?.SetActive(true);
        player?.SetInteracting(true);

        if (plantingTitle) plantingTitle.text = "O que plantar?";
        foreach (Transform t in cropGrid) Destroy(t.gameObject);

        foreach (var crop in GameManager.Instance.availableCrops)
        {
            var go = Instantiate(cropButtonPrefab, cropGrid);
            var btn = go.GetComponent<Button>();
            var nameLabel = go.transform.Find("CropName")?.GetComponent<TextMeshProUGUI>();
            var costLabel = go.transform.Find("CostText")?.GetComponent<TextMeshProUGUI>();
            var timeLabel = go.transform.Find("TimeText")?.GetComponent<TextMeshProUGUI>();

            if (nameLabel) nameLabel.text = crop.cropName;
            if (costLabel) costLabel.text = $"{crop.seedCost} moedas";
            if (timeLabel) timeLabel.text = $"{crop.growthTimeSeconds}s crescimento";

            var cropRef = crop;
            btn?.onClick.AddListener(() => PlantCrop(cropRef));
        }
    }

    void PlantCrop(CropData crop)
    {
        if (selectedPlot == null) return;
        if (selectedPlot.TryPlant(crop))
        {
            ClosePlantingMenu();
        }
        else
        {
            ShowNotification("Moedas insuficientes!");
        }
    }

    void ClosePlantingMenu()
    {
        plantingPanel?.SetActive(false);
        selectedPlot = null;
        player?.SetInteracting(false);
    }

    // === LOJA ===
    public void OpenShopPanel(NPCTrader trader)
    {
        currentTrader = trader;
        CloseAll();
        shopPanel?.SetActive(true);
        player?.SetInteracting(true);

        if (shopTitle) shopTitle.text = trader.npcName;
        if (shopGreeting) shopGreeting.text = trader.greeting;

        BuildShopList(buyOffersGrid, trader.buyOffers, true);
        BuildShopList(sellOffersGrid, trader.sellOffers, false);
    }

    void BuildShopList(Transform grid, List<ShopOffer> offers, bool isSelling)
    {
        if (grid == null) return;
        foreach (Transform t in grid) Destroy(t.gameObject);

        foreach (var offer in offers)
        {
            var go = Instantiate(shopItemPrefab, grid);
            var nameLabel = go.transform.Find("ItemName")?.GetComponent<TextMeshProUGUI>();
            var priceLabel = go.transform.Find("PriceText")?.GetComponent<TextMeshProUGUI>();
            var btn = go.GetComponentInChildren<Button>();

            if (nameLabel) nameLabel.text = offer.itemName;
            if (priceLabel) priceLabel.text = isSelling
                ? $"Vender por {offer.priceInCoins} moedas"
                : $"Comprar por {offer.priceInCoins} moedas";

            var offerRef = offer;
            btn?.onClick.AddListener(() =>
            {
                if (isSelling) currentTrader.BuyFromPlayer(offerRef.itemId, 1);
                else currentTrader.SellToPlayer(offerRef.itemId, 1);
            });
        }
    }

    void CloseShop()
    {
        shopPanel?.SetActive(false);
        currentTrader = null;
        player?.SetInteracting(false);
    }

    void CloseAll()
    {
        inventoryPanel?.SetActive(false);
        plantingPanel?.SetActive(false);
        shopPanel?.SetActive(false);
    }
}
