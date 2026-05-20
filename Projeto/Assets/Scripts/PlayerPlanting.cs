using UnityEngine;

public class PlayerPlanting : MonoBehaviour
{
    public Camera cam;

    public float interactDistance = 5f;

    public GameObject seedPrefab;

    void Update()
    {
        // Plantar
        if (Input.GetKeyDown(KeyCode.P))
        {
            TryPlant();
        }

        // Colher
        if (Input.GetKeyDown(KeyCode.C))
        {
            TryHarvest();
        }
    }

    void TryPlant()
    {
        FarmTile tile = GetTargetTile();

        if (tile == null) return;

        if (!tile.hasPlant)
        {
            tile.PlantSeed(seedPrefab);

            Debug.Log("Plantou!");
        }
        else
        {
            Debug.Log("Já existe planta aqui.");
        }
    }

    void TryHarvest()
    {
        FarmTile tile = GetTargetTile();

        if (tile == null) return;

        if (tile.hasPlant)
        {
            tile.HarvestPlant();
        }
    }

    FarmTile GetTargetTile()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // return hit.collider.GetComponent<FarmTile>();
            return hit.collider.GetComponentInParent<FarmTile>();
        }

        return null;
    }
}
/*using UnityEngine;

public class PlayerPlanting : MonoBehaviour
{
    public Camera cam;

    public float interactDistance = 5f;

    public GameObject seedPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                FarmTile tile = hit.collider.GetComponent<FarmTile>();

                if (tile != null)
                {
                    if (!tile.hasPlant)
                    {
                        tile.PlantSeed(seedPrefab);
                    }
                    else
                    {
                        tile.HarvestPlant();
                    }
                }
            }
        }
    }
}*/