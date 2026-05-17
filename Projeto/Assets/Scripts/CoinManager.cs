using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    private int coins = 0;

    void Start()
    {
        AddCoins(100);
    }

    public void AddCoins(int amount)
    {
        coins += amount;

        UpdateUI();

        Debug.Log("Moedas: " + coins);
    }

    public bool RemoveCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;

            UpdateUI();

            Debug.Log("Moedas: " + coins);

            return true;
        }

        Debug.Log("Moedas insuficientes.");

        return false;
    }

    void UpdateUI()
    {
        coinText.text = coins.ToString();
    }
}