using UnityEngine;

public enum PlotState { Empty, Planted, Growing, Ready }

public class FarmPlot : MonoBehaviour
{
    [Header("Plot Settings")]
    public int plotIndex;

    [Header("Visual Objects")]
    public GameObject soilMesh;
    public GameObject seedVisual;
    public GameObject growingVisual;
    public GameObject readyVisual;
    public GameObject highlightEffect;
    public ParticleSystem harvestParticles;

    [Header("Interaction")]
    public float interactRadius = 1.5f;

    private PlotState state = PlotState.Empty;
    private CropData currentCrop;
    private float growthTimer;
    private bool playerNearby;

    public PlotState State => state;
    public CropData CurrentCrop => currentCrop;

    void Update()
    {
        if (state == PlotState.Growing)
        {
            growthTimer -= Time.deltaTime;
            if (growthTimer <= 0f)
            {
                SetState(PlotState.Ready);
            }
        }

        if (highlightEffect != null)
            highlightEffect.SetActive(playerNearby);
    }

    public bool TryPlant(CropData crop)
    {
        if (state != PlotState.Empty) return false;
        if (!GameManager.Instance.SpendCoins(crop.seedCost)) return false;

        currentCrop = crop;
        growthTimer = crop.growthTimeSeconds;
        SetState(PlotState.Planted);

        // Transição visual rápida para Growing
        Invoke(nameof(StartGrowing), 1f);

        UIManager.Instance?.ShowNotification($"Plantou {crop.cropName}!");
        return true;
    }

    void StartGrowing() => SetState(PlotState.Growing);

    public void Harvest()
    {
        if (state != PlotState.Ready || currentCrop == null) return;

        harvestParticles?.Play();
        GameManager.Instance.AddToInventory(
            currentCrop.name,
            currentCrop.cropName,
            1,
            currentCrop.sellPrice
        );

        UIManager.Instance?.ShowNotification($"Colheu {currentCrop.cropName}! +1");

        currentCrop = null;
        SetState(PlotState.Empty);
    }

    void SetState(PlotState newState)
    {
        state = newState;
        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        seedVisual?.SetActive(state == PlotState.Planted);
        growingVisual?.SetActive(state == PlotState.Growing);
        readyVisual?.SetActive(state == PlotState.Ready);

        // Muda cor do solo
        if (soilMesh != null)
        {
            var rend = soilMesh.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = state == PlotState.Empty
                    ? new Color(0.5f, 0.35f, 0.2f)
                    : new Color(0.35f, 0.22f, 0.1f);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNearby = false;
    }

    public bool IsPlayerNearby() => playerNearby;

    public float GetGrowthProgress()
    {
        if (currentCrop == null) return 0f;
        if (state == PlotState.Ready) return 1f;
        return 1f - (growthTimer / currentCrop.growthTimeSeconds);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
#endif
}
