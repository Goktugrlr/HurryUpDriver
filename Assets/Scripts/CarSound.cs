using UnityEngine;

public class CarSounds : MonoBehaviour
{
    public float minSpeed;
    public float maxSpeed;
    private float currentSpeed;

    private Rigidbody carRb;
    private AudioSource carAudio;
    public AudioSource nitroAudio;
    public AudioSource handbrakeAudio;
    public AudioSource truckHorn;
    public AudioSource safetyAudio;

    public float minPitch;
    public float maxPitch;
    private float pitchFromCar;

    public bool isNitroUsing = false;

    void Start()
    {
        carAudio = GetComponent<AudioSource>();
        carRb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        EngineSound();
        TruckHorn();
        HandbrakeAudio();
        SafetyAudio();
    }

    void EngineSound()
    {
        currentSpeed = carRb.velocity.magnitude;
        pitchFromCar = carRb.velocity.magnitude / 60f;

        if (currentSpeed < minSpeed)
        {
            carAudio.pitch = minPitch;
        }

        if (currentSpeed > minSpeed && currentSpeed < maxSpeed)
        {
            carAudio.pitch = minPitch + pitchFromCar;
        }

        if (currentSpeed > maxSpeed)
        {
            carAudio.pitch = maxPitch;
        }
    }


    public void NitroSFX(bool isUsingNitro)
    {
        if (isUsingNitro && !nitroAudio.isPlaying)
        {
            nitroAudio.Play();
        }
        else if (!isUsingNitro && nitroAudio.isPlaying)
        {
            nitroAudio.Stop();
        }
    }


    void TruckHorn()
    {
        if (Input.GetKey(KeyCode.H))
        {
            if (!truckHorn.isPlaying)
            {
                truckHorn.Play();
            }
        }
        else
        {
            if (truckHorn.isPlaying)
            {
                truckHorn.Stop();
            }
        }
    }

    void HandbrakeAudio()
    {
        if (Input.GetKey(KeyCode.Space) && gameObject.GetComponent<Rigidbody>().velocity.magnitude >= 10)
        {
            if (!handbrakeAudio.isPlaying)
            {
                handbrakeAudio.Play();
            }
        }
        else
        {
            if (handbrakeAudio.isPlaying)
            {
                handbrakeAudio.Stop();
            }
        }
    }

    void SafetyAudio()
    {
        if (Input.GetAxis("Vertical") < -0.1f)
        {
            if(!safetyAudio.isPlaying)
            safetyAudio.Play();
        }
        else
        {
            safetyAudio.Stop();
        }
    }
}