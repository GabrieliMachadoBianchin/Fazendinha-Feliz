using UnityEngine;

/// Quadrado de terra onde o jogador planta e colhe cenouras.
public class FarmTile : MonoBehaviour
{
    public bool hasPlant = false;
    public GameObject currentPlant;
    public Transform plantPoint;

    public void PlantSeed(GameObject seedPrefab)
    {
        if (hasPlant) return;

        Vector3 pos = plantPoint != null ? plantPoint.position : transform.position;
        currentPlant = Instantiate(seedPrefab, pos, Quaternion.identity);
        hasPlant = true;
        Debug.Log("[FarmTile] Cenoura plantada.");
    }

    public void HarvestPlant()
    {
        if (!hasPlant || currentPlant == null) return;

        Plant plant = currentPlant.GetComponentInChildren<Plant>();
        if (plant == null) return;

        if (!plant.readyToHarvest)
        {
            Debug.Log("[FarmTile] Cenoura ainda não cresceu.");
            return;
        }

        plant.Harvest();
        Destroy(currentPlant);
        currentPlant = null;
        hasPlant = false;
        Debug.Log("[FarmTile] Cenoura colhida!");
    }
}