using UnityEngine;
using TMPro;

/// Armazém do jogador. Guarda apenas cenouras.
public class StorageManager : MonoBehaviour
{
    public static StorageManager Instance;

    [Header("UI")]
    public GameObject storageUI;
    public TextMeshProUGUI carrotCountText;
    public UnityEngine.UI.Button closeButton;

    private int carrots = 0;
    private bool isOpen = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Debug.Log("[StorageManager] Start() rodou! carrots=" + carrots, gameObject);
        if (closeButton != null)
            closeButton.onClick.AddListener(CloseStorage);

        
        storageUI.SetActive(false);
        // NÃO chama UpdateUI aqui — o painel está inativo e o texto não atualiza
    }

    public void OpenStorage()
    {
        if (isOpen) return;
        isOpen = true;
        storageUI.SetActive(true); // ativa o painel PRIMEIRO
        UpdateUI();                // agora o QtCenouras está ativo, texto atualiza
    }

    public void CloseStorage()
    {
        isOpen = false;
        storageUI.SetActive(false);
    }

    public bool IsOpen() => isOpen;

    public void AddCarrots(int amount)
    {
        carrots += amount;
        // Debug.Log($"[Armazém] +{amount} cenoura(s). Total: {carrots}");
        // só atualiza o texto se o painel estiver aberto
        // if (isOpen) UpdateUI();
        UpdateUI();
    }

    public bool RemoveCarrots(int amount)
    {
        if (carrots < amount)
        {
            // Debug.LogWarning($"[Armazém] Cenouras insuficientes. Tem {carrots}, precisa de {amount}.");
            return false;
        }
        carrots -= amount;
        // Debug.Log($"[Armazém] -{amount} cenoura(s). Total: {carrots}");
        // if (isOpen) UpdateUI();
        UpdateUI();
        return true;
    }

    public int GetCarrots() => carrots;

    void UpdateUI()
{
    // Debug.Log("[UI] UpdateUI chamado. carrots=" + carrots + " | texto null? " + (carrotCountText == null));
    if (carrotCountText != null)
        carrotCountText.text = "Cenoura x " + carrots;
}

/*
    void UpdateUI()
    {
        if (carrotCountText != null)
            carrotCountText.text = "Cenoura x" + carrots;
    }*/
}