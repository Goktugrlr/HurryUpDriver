using UnityEngine;
using System;
using System.Collections.Generic;
public class Movement : MonoBehaviour
{
    public enum Axel
    {
        front,
        rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public GameObject wheelEffect;
        public ParticleSystem smokeEffect;
        public Axel axel;
    }

    public float maxAcceleration = 30f;
    public float brakeAcceleration = 50f;

    public float turnSensitivity = 0.75f;
    public float maxSteerAngle = 25f;

    public List<Wheel> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody rb;

    public Vector3 centerOfMass;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
    }


    private void Update()
    {
        GetInput();    
        AnimateWheels();
        ApplyEffects();
    }

    void GetInput()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");

        //Debug.Log("Move Input: " + moveInput);
        //Debug.Log("Steer Input: " + steerInput);
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
}
