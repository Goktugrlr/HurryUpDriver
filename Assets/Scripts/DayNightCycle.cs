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
        streetLamps = GameObject.FindGameObjectsWithTag("lamp");
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
        bool isNighttime = currentTimeOfDay >= 0.5f && currentTimeOfDay <= 0.75f;

        foreach (GameObject lamps in streetLamps)
        {
            Light lamp = lamps.GetComponent<Light>();
            lamp.intensity = isNighttime ? 1f : 0f;
        }
    }
}