using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    private int coins = 0;

    void Start()
    {
        AddCoins(100);
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        coins += amount;

        UpdateUI();

        Debug.Log("Moedas: " + coins);
    }

    void UpdateUI()
    {
        coinText.text = coins.ToString();
    }
}