using UnityEngine;

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
}