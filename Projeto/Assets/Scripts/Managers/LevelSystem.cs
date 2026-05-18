using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSystem : MonoBehaviour
{
    [Header("UI")]
    public Slider xpBar;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI xpText;

    [Header("XP por Ação")]
    public float xpPerHarvest = 20f;
    public float xpPerSell = 10f;
    public float xpPerBuy = 5f;

    [Header("Level Up")]
    public GameObject levelUpEffect;
    public AudioClip levelUpSound;

    private int level = 1;
    private float currentXP = 0;
    private float requiredXP = 100;
    private AudioSource audioSource;

    public int Level => level;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Escuta eventos do GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnHarvest += OnHarvest;
            GameManager.Instance.OnSell += OnSell;
            GameManager.Instance.OnBuy += OnBuy;
        }
        else
        {
            Debug.LogWarning("LevelSystem: GameManager não encontrado na cena.");
        }

        UpdateUI();
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnHarvest -= OnHarvest;
            GameManager.Instance.OnSell -= OnSell;
            GameManager.Instance.OnBuy -= OnBuy;
        }
    }

    // === CALLBACKS DOS EVENTOS ===
    void OnHarvest(string cropName) => AddXP(xpPerHarvest, $"+{xpPerHarvest} XP (colheita)");
    void OnSell(int coinsEarned)    => AddXP(xpPerSell,    $"+{xpPerSell} XP (venda)");
    void OnBuy(int coinsSpent)      => AddXP(xpPerBuy,     $"+{xpPerBuy} XP (compra)");

    // === XP ===
    public void AddXP(float amount, string hint = "")
    {
        currentXP += amount;

        if (!string.IsNullOrEmpty(hint))
            UIManager.Instance?.ShowNotification(hint);

        while (currentXP >= requiredXP)
        {
            currentXP -= requiredXP;
            LevelUp();
        }

        UpdateUI();
    }

    void LevelUp()
    {
        level++;
        requiredXP *= 1.5f;

        UIManager.Instance?.ShowNotification($"🎉 Level {level}! Parabéns!");

        if (levelUpEffect != null)
        {
            var fx = Instantiate(levelUpEffect, transform.position, Quaternion.identity);
            Destroy(fx, 3f);
        }

        if (levelUpSound != null && audioSource != null)
            audioSource.PlayOneShot(levelUpSound);

        // Recompensa a cada level: bônus de moedas
        int coinBonus = level * 10;
        GameManager.Instance?.AddCoins(coinBonus);
        UIManager.Instance?.ShowNotification($"Bônus de level: +{coinBonus} moedas!");

        Debug.Log($"[LevelSystem] Subiu para o nível {level}! XP necessário: {requiredXP}");
    }

    void UpdateUI()
    {
        if (xpBar != null)
        {
            xpBar.maxValue = requiredXP;
            xpBar.value = currentXP;
        }

        if (levelText != null)
            levelText.text = "Lv " + level;

        if (xpText != null)
            xpText.text = $"{Mathf.RoundToInt(currentXP)} / {Mathf.RoundToInt(requiredXP)} XP";
    }

    // === UTILIDADE ===
    public float GetXPProgress() => currentXP / requiredXP;
}
