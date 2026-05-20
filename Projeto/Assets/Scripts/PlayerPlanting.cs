using UnityEngine;

/// P = plantar cenoura | C = colher cenoura
public class PlayerPlanting : MonoBehaviour
{
    public Camera cam;
    public GameObject seedPrefab;
    public float interactDistance = 5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) TryPlant();
        if (Input.GetKeyDown(KeyCode.C)) TryHarvest();
    }

    void TryPlant()
    {
        FarmTile tile = GetTargetTile();
        if (tile == null) return;

        if (!tile.hasPlant)
            tile.PlantSeed(seedPrefab);
        else
            Debug.Log("[Player] Já tem cenoura aqui.");
    }

    void TryHarvest()
    {
        FarmTile tile = GetTargetTile();
        if (tile == null) return;

        if (tile.hasPlant)
            tile.HarvestPlant();
    }

    FarmTile GetTargetTile()
    {
        if (cam == null) return null;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, interactDistance))
            return hit.collider.GetComponentInParent<FarmTile>();
        return null;
    }
}