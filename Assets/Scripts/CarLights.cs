using UnityEngine;

public class CarLights : MonoBehaviour
{
    public bool isBackLightOn;
    private bool isHeadLightOn;
    public GameObject[] backLights;
    public GameObject[] headLights;

    private Color originalLightColor;
    private Color originalParticleSystemColor;

    void Start()
    {
        isBackLightOn = false;
        originalLightColor = Color.red;
        originalParticleSystemColor = Color.red;
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
                Rigidbody vehicleRigidbody = gameObject.GetComponent<Rigidbody>();

                if (Vector3.Dot(vehicleRigidbody.velocity.normalized, transform.forward) < -0.1f)
                {
                    light.GetComponentInChildren<Light>().color = Color.white;

                    var mainModule = light.GetComponentInChildren<ParticleSystem>().main;
                    mainModule.startColor = Color.white;
                }
                else
                {
                    light.GetComponentInChildren<Light>().color = originalLightColor;

                    var mainModule = light.GetComponentInChildren<ParticleSystem>().main;
                    mainModule.startColor = originalParticleSystemColor;
                }
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
