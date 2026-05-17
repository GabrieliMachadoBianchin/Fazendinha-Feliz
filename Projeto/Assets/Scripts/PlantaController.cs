using UnityEngine;

public class Plant : MonoBehaviour
{
    public float growTime = 10f;

    private float timer;

    public bool readyToHarvest = false;

    void Update()
    {
        if (readyToHarvest) return;

        timer += Time.deltaTime;

        if (timer >= growTime)
        {
            readyToHarvest = true;

            Debug.Log("Planta pronta para colher!");
        }
    }
}