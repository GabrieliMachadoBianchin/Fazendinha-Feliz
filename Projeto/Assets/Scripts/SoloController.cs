using UnityEngine;

public class FarmTile : MonoBehaviour
{
    public bool hasPlant = false;

    public GameObject currentPlant;

    public Transform plantPoint;

    public void PlantSeed(GameObject seedPrefab)
    {
        if (hasPlant) return;

        currentPlant = Instantiate(
            seedPrefab,
            plantPoint.position,
            Quaternion.identity
        );

        hasPlant = true;
    }

    public void HarvestPlant()
    {
        if (!hasPlant) return;

        Plant plant = currentPlant.GetComponent<Plant>();

        if (plant.readyToHarvest)
        {
            Destroy(currentPlant);

            hasPlant = false;

            Debug.Log("Planta colhida!");
        }
        else
        {
            Debug.Log("Ainda não cresceu.");
        }
    }
}