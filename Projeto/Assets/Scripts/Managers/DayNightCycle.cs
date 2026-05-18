using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Day Length")]
    public float dayLengthSeconds = 120f;

    [Header("Sun")]
    public Light sunLight;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Ambient")]
    public Gradient ambientColor;

    private float currentTime = 0.25f; // Começa de manhã

    public float TimeOfDay => currentTime; // 0=meia-noite, 0.5=meio-dia
    public bool IsDay => currentTime > 0.2f && currentTime < 0.8f;

    void Update()
    {
        currentTime += Time.deltaTime / dayLengthSeconds;
        if (currentTime >= 1f) currentTime = 0f;

        UpdateSun();
        UpdateAmbient();
    }

    void UpdateSun()
    {
        if (sunLight == null) return;
        float angle = currentTime * 360f - 90f;
        sunLight.transform.rotation = Quaternion.Euler(angle, -30f, 0);
        sunLight.color = sunColor.Evaluate(currentTime);
        sunLight.intensity = sunIntensity.Evaluate(currentTime);
    }

    void UpdateAmbient()
    {
        RenderSettings.ambientLight = ambientColor.Evaluate(currentTime);
    }

    public string GetTimeString()
    {
        int hours = Mathf.FloorToInt(currentTime * 24f);
        int minutes = Mathf.FloorToInt((currentTime * 24f - hours) * 60f);
        return $"{hours:D2}:{minutes:D2}";
    }
}
