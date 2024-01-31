using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float dayDuration = 240f; 
    private float currentTimeOfDay = 0f; 

    public Light directionalLight; 
    public Gradient dayNightColorGradient; 

    void Update()
    {
        UpdateTimeOfDay();
        UpdateSunRotation();
        UpdateLightColor();
    }

    void UpdateTimeOfDay()
    {
        currentTimeOfDay += Time.deltaTime / dayDuration;

        if (currentTimeOfDay >= 1f)
        {
            currentTimeOfDay = 0f; 
        }
    }

    void UpdateSunRotation()
    {
        float angle = currentTimeOfDay * 360f; 
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3(angle, 0, 0));
    }

    void UpdateLightColor()
    {
        directionalLight.color = dayNightColorGradient.Evaluate(currentTimeOfDay);
    }
}