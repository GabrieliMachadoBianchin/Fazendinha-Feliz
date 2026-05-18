using UnityEngine;

/// <summary>
/// Adiciona efeito de pulsação ao plot quando o jogador está perto.
/// Anexe no GameObject do highlight (anel no chão).
/// </summary>
public class PlotHighlight : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public float minAlpha = 0.3f;
    public float maxAlpha = 0.9f;

    private Renderer rend;
    private MaterialPropertyBlock mpb;

    void Start()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    void Update()
    {
        if (rend == null) return;
        float alpha = Mathf.Lerp(minAlpha, maxAlpha,
            (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);

        rend.GetPropertyBlock(mpb);
        mpb.SetFloat("_Alpha", alpha);
        rend.SetPropertyBlock(mpb);
    }
}
