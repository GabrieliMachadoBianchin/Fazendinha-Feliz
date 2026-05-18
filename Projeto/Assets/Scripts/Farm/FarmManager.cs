using UnityEngine;
using System.Collections.Generic;

public class FarmManager : MonoBehaviour
{
    public static FarmManager Instance { get; private set; }

    [Header("Farm Grid")]
    public int rows = 3;
    public int cols = 4;
    public float spacing = 2.5f;
    public GameObject farmPlotPrefab;
    public Transform farmOrigin;

    private List<FarmPlot> plots = new List<FarmPlot>();
    public List<FarmPlot> Plots => plots;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        GenerateFarm();
    }

    void GenerateFarm()
    {
        Vector3 origin = farmOrigin != null ? farmOrigin.position : Vector3.zero;
        int idx = 0;
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                Vector3 pos = origin + new Vector3(c * spacing, 0, r * spacing);
                GameObject plotObj = Instantiate(farmPlotPrefab, pos, Quaternion.identity, transform);
                plotObj.name = $"Plot_{r}_{c}";
                var plot = plotObj.GetComponent<FarmPlot>();
                if (plot != null)
                {
                    plot.plotIndex = idx++;
                    plots.Add(plot);
                }
            }
        }
    }

    public FarmPlot GetNearestEmptyPlot(Vector3 playerPos)
    {
        FarmPlot nearest = null;
        float minDist = float.MaxValue;
        foreach (var p in plots)
        {
            if (p.State == PlotState.Empty)
            {
                float d = Vector3.Distance(playerPos, p.transform.position);
                if (d < minDist) { minDist = d; nearest = p; }
            }
        }
        return nearest;
    }

    public FarmPlot GetNearestReadyPlot(Vector3 playerPos)
    {
        FarmPlot nearest = null;
        float minDist = float.MaxValue;
        foreach (var p in plots)
        {
            if (p.State == PlotState.Ready)
            {
                float d = Vector3.Distance(playerPos, p.transform.position);
                if (d < minDist) { minDist = d; nearest = p; }
            }
        }
        return nearest;
    }

    public int GetTotalReady() => plots.FindAll(p => p.State == PlotState.Ready).Count;
    public int GetTotalPlanted() => plots.FindAll(p => p.State != PlotState.Empty).Count;
}
