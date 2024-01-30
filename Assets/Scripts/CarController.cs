using UnityEngine;
using System;
using System.Collections.Generic;
public class CarController : MonoBehaviour
{
    public enum Axel
    {
        front,
        rear
    }

    private float fuelCapacity = 100f;
    private float nitrousCapacity = 30f;
    private CarLights carLights;
    public GameManager gameManager;

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffect;
        public ParticleSystem smokeEffect;
        public Axel axel;
    }

    public float maxAcceleration = 10000f;
    private float accWithNitrous = 20000f;
    public float brakeAcceleration = 50f;

    public float turnSensitivity = 0.75f;
    public float maxSteerAngle = 25f;

    public List<Wheel> wheels;
    public ParticleSystem NitrousEffect1;
    public ParticleSystem NitrousEffect2;
    float moveInput;
    float steerInput;

    private Rigidbody rb;

    public Vector3 centerOfMass;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
        gameManager.SetMaxFuelCapacity(fuelCapacity);
        gameManager.SetMaxNitrousCapacity(nitrousCapacity);
        carLights = GetComponent<CarLights>();
    }


    private void Update()
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
    }

    void GetInput()
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

    private void FixedUpdate()
    {
        Move();    
        Steer();
        HandBrake();
    }

    void Move()
    {
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * maxAcceleration * Time.deltaTime;
        }

        carLights.isBackLightOn = (moveInput < 0);

        carLights.OperateBackLights();
    }

    void Steer()
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

    void AnimateWheels()
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

    void HandBrake()
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

    void ApplyEffects()
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

            if (100 - fuelCapacity < 20)
            {
                fuelCapacity = 100;
            } 
            else
            {
                fuelCapacity += 20;
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
            return false;
        }
        else
        {
            return true;
        }
    }

    public void HandleNitrous()
    {      
        if (Input.GetKey(KeyCode.LeftShift) && CheckNitrous())
        {
            maxAcceleration = accWithNitrous;
            nitrousCapacity -= 3f * Time.deltaTime;
            NitrousEffect1.Play();
            NitrousEffect2.Play();
        }
        else
        {
            maxAcceleration = 10000f;
            NitrousEffect1.Stop();
            NitrousEffect2.Stop();
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

}
