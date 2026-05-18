using UnityEngine;

[CreateAssetMenu(fileName = "NewCrop", menuName = "Fazenda/Crop Data")]
public class CropData : ScriptableObject
{
    [Header("Informações Básicas")]
    public string cropName;
    public Sprite seedIcon;
    public Sprite harvestIcon;
    public GameObject seedPrefab;
    public GameObject growingPrefab;
    public GameObject readyPrefab;

    [Header("Crescimento")]
    public float growthTimeSeconds = 30f;
    public int seedCost = 5;
    public int sellPrice = 15;

    [Header("Visual")]
    public Color cropColor = Color.green;
}
