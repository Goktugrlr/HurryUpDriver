using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float dayDuration = 240f; 
    private float currentTimeOfDay = 0f; 

    public Light directionalLight; 
    public Gradient dayNightColorGradient; 

    private GameObject[] streetLamps;

    private void Start()
    {
        streetLamps = GameObject.FindGameObjectsWithTag("Lamp");
    }
    void Update()
    {
        UpdateTimeOfDay();
        UpdateSunRotation();
        UpdateLightColor();
        UpdateLamps();
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

    void UpdateLamps()
    {
        float nightStart = 0.5f;
        float nightEnd = 0.9f;

        bool isNighttime = currentTimeOfDay >= nightStart && currentTimeOfDay <= nightEnd;

        foreach (GameObject lamps in streetLamps)
        {
            Light lamp = lamps.GetComponentInChildren<Light>();
            lamp.intensity = isNighttime ? 9f : 0f;
        }
    }
}