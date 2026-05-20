using UnityEngine;

/// Planta de cenoura. Ao colher, adiciona cenouras ao armazém.
public class Plant : MonoBehaviour
{
    public float growTime = 10f;
    public int harvestAmount = 1;

    private float timer;
    public bool readyToHarvest = false;

    void Update()
    {
        if (readyToHarvest) return;

        timer += Time.deltaTime;
        if (timer >= growTime)
        {
            readyToHarvest = true;
            // Debug.Log("[Planta] Cenoura pronta para colher!");
        }
    }


    public bool Harvest()
{
    if (!readyToHarvest) return false;

    StorageManager.Instance.AddCarrots(harvestAmount);
    return true;
}
}
