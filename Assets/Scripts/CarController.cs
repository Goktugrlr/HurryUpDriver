using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
public class CarController : MonoBehaviour
{
    public enum Axel
    {
        front,
        rear
    }

    private float fuelCapacity = 180f;
    private float nitrousCapacity = 30f;
    private CarLights carLights;
    public GameManager gameManager;
    private CarSounds carSounds;
    public Text fuelDepleted;

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffect;
        public ParticleSystem smokeEffect;
        public ParticleSystem[] nitroEffects;
        public Axel axel;
    }

    public float maxAcceleration = 10000f;
    private float accWithNitrous = 20000f;
    public float brakeAcceleration = 60f;

    public float turnSensitivity = 0.75f;
    public float maxSteerAngle = 25f;
    public Vector3 centerOfMass;
    public List<Wheel> wheels;
    public ParticleSystem NitrousEffect1;
    public ParticleSystem NitrousEffect2;

    private float moveInput;
    private float steerInput;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
        gameManager.SetMaxFuelCapacity(fuelCapacity);
        gameManager.SetMaxNitrousCapacity(nitrousCapacity);
        carLights = GetComponent<CarLights>();
        carSounds = GetComponent<CarSounds>();
    }

    void Update()
    {
        GetInput();
        AnimateWheels();
        ApplyEffects();

        if (CheckFuel())
        {
        fuelCapacity -= 1f * Time.deltaTime;
        gameManager.SetFuelCapacity(fuelCapacity);
        }

        HandleNitrous();
        RespawnVehicle();
    }

    private void GetInput()
    {
        if (CheckFuel())
        {
            moveInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
        }
        else
        {
            moveInput = 0f;
            steerInput = Input.GetAxis("Horizontal");
        }
    }

    void FixedUpdate()
    {
        Move();    
        Steer();
        HandBrake();
    }

    private void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * maxAcceleration * Time.deltaTime;
        }

        carLights.isBackLightOn = (moveInput < 0);
        carLights.OperateBackLights();
    }

    private void Steer()
    {
        foreach (var wheel in wheels)
        {
            if(wheel.axel == Axel.front)
            {
                var steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
            }
        }
    }

    private void AnimateWheels()
    {
        foreach(var wheel in wheels)
        {
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }

    private void HandBrake()
    {
        foreach (var wheel in wheels)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                wheel.wheelCollider.brakeTorque = Mathf.Infinity;
            }
            else
            {
                wheel.wheelCollider.brakeTorque = 0f;
            }
        }
    }

    private void ApplyEffects()
    {
        foreach (var wheel in wheels)
        {
            if (Input.GetKey(KeyCode.Space) && wheel.axel == Axel.rear && wheel.wheelCollider.isGrounded == true && rb.velocity.magnitude >= 10f)
            {
                wheel.wheelEffect.GetComponentInChildren<TrailRenderer>().emitting = true;
                wheel.smokeEffect.Emit(1);
            }
            else
            {
                wheel.wheelEffect.GetComponentInChildren<TrailRenderer>().emitting = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fuel"))
        {
            Vector3 destroyedFuelCanPosition = other.transform.parent.position;

            Destroy(other.transform.parent.gameObject);

            if (180 - fuelCapacity < 30)
            {
                fuelCapacity = 180;
            } 
            else
            {
                fuelCapacity += 30;
            }
            
            FindObjectOfType<Pickable>().RespawnFuel(destroyedFuelCanPosition);  
        }
        
        if (other.CompareTag("Nitro"))
        {
            Destroy(other.transform.parent.gameObject);

            nitrousCapacity = 30;

            FindObjectOfType<Pickable>().SpawnNitro();
        }
    }

    public bool CheckFuel()
    {
        if (fuelCapacity <= 0)
        {
            fuelCapacity = 0;
            fuelDepleted.gameObject.SetActive(true);
            return false;
        }
        else
        {
            fuelDepleted.gameObject.SetActive(false);
            return true;
        }
    }

    private void HandleNitrous()
    {      
        if (Input.GetKey(KeyCode.LeftShift) && CheckNitrous())
        {
            maxAcceleration = accWithNitrous;
            nitrousCapacity -= 3f * Time.deltaTime;
            NitrousEffect1.gameObject.SetActive(true);
            NitrousEffect2.gameObject.SetActive(true);

            carSounds.NitroAudio(true);
        }
        else
        {
            maxAcceleration = 10000f;
            NitrousEffect1.gameObject.SetActive(false);
            NitrousEffect2.gameObject.SetActive(false);

            carSounds.NitroAudio(false);
        }
        gameManager.SetNitrousCapacity(nitrousCapacity);
    }

    private bool CheckNitrous()
    {
        if (nitrousCapacity <= 0)
        {
            nitrousCapacity = 0;
            return false;
        }
        else
        {
            return true;
        }
    }

    private void RespawnVehicle()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GameObject vehicle = FindObjectOfType<CarController>().gameObject;
            vehicle.GetComponent<Rigidbody>().velocity = Vector3.zero;
            Vector3 respawnPositon = new Vector3(120, 5, 220);
            vehicle.transform.position = respawnPositon;

            Quaternion respawnRotation = Quaternion.Euler(0f, 180f, 0f); 
            vehicle.transform.rotation = respawnRotation;
        }
    }
}