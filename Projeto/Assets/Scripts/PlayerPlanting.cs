using UnityEngine;

public class PlayerPlanting : MonoBehaviour
{
    public Camera cam;

    public float interactDistance = 5f;

    public GameObject seedPrefab;

    private FarmTile selectedTile;

    void Update()
    {
        SelecionarTerreno();

        Plantar();

        Colher();
    }

    void SelecionarTerreno()
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
                    selectedTile = tile;

                    Debug.Log("Terreno selecionado.");
                }
            }
        }
    }

    void Plantar()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (selectedTile != null)
            {
                if (!selectedTile.hasPlant)
                {
                    selectedTile.PlantSeed(seedPrefab);
                }
            }
        }
    }

    void Colher()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                FarmTile tile = hit.collider.GetComponent<FarmTile>();

                if (tile != null)
                {
                    tile.HarvestPlant();
                }
            }
        }
    }
}