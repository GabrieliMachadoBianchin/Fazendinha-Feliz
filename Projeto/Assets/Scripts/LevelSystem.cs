using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSystem : MonoBehaviour
{
    public Slider xpBar;
    public TextMeshProUGUI levelText;

    private int level = 1;

    private float currentXP = 0;

    private float requiredXP = 100;

    void Start()
    {
        UpdateUI();
    }

    public void AddXP(float amount)
    {
        currentXP += amount;

        while (currentXP >= requiredXP)
        {
            currentXP -= requiredXP;

            LevelUp();
        }

        UpdateUI();

        Debug.Log("XP Atual: " + currentXP);
    }

    void LevelUp()
    {
        level++;

        requiredXP *= 1.5f;

        Debug.Log("SUBIU PARA O NÍVEL " + level);
    }

    void UpdateUI()
    {
        xpBar.maxValue = requiredXP;

        xpBar.value = currentXP;

        levelText.text = "Lv " + level;
    }
}