using UnityEngine;

public class CarLights : MonoBehaviour
{
    public bool isBackLightOn;
    private bool isHeadLightOn;
    public GameObject[] backLights;
    public GameObject[] headLights;

    void Start()
    {
        isBackLightOn = false;
    }

    private void Update()
    {
        TurnOnHeadLights();
    }

    public void OperateBackLights()
    {
        if (isBackLightOn)
        {
            foreach (var light in backLights)
            {
                light.SetActive(true);
            }
        }
        else
        {
            foreach (var light in backLights)
            {
                light.SetActive(false);
            }
        }
    }
    
    private void TurnOnHeadLights()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            if (isHeadLightOn)
            {
                foreach (var light in headLights)
                {
                    light.SetActive(false);
                    isHeadLightOn = false;
                }
            }
            else
            {
                foreach (var light in headLights)
                {
                    light.SetActive(true);
                    isHeadLightOn = true;
                }
            }
        }
    }
}
