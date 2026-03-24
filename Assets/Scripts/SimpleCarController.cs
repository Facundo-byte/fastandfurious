using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}
     
public class SimpleCarController : MonoBehaviour {
    public List<AxleInfo> axleInfos; 
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float maxBrakeTorque = 3000f;
    public float idleBrakeTorque = 500f;

    public float motor;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0) {
            return;
        }
     
        Transform visualWheel = collider.transform.GetChild(0);
     
        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);
     
        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }
     
    public void FixedUpdate()
    {
        motor = maxMotorTorque * Input.GetAxis("Vertical");
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        bool isBraking;
        bool handbrake;

        if(Input.GetKey(KeyCode.Space)){
            isBraking = true;
            handbrake = true;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            isBraking = true;
            handbrake = false;
        }
        else
        {
            handbrake = false;
            isBraking = false;
        }
        
        foreach (AxleInfo axleInfo in axleInfos) {

            //direccion
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }

            //motor
            if (axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

                 // FRENO
            if (isBraking)
            {
                if (!handbrake && Vector3.Dot(rb.linearVelocity, transform.forward) > 0.5f)
                {
                    axleInfo.leftWheel.brakeTorque = maxBrakeTorque;
                    axleInfo.rightWheel.brakeTorque = maxBrakeTorque;
                }
                else if(Vector3.Dot(rb.linearVelocity, transform.forward) > 0.5f)
                {
                    axleInfo.leftWheel.brakeTorque = maxBrakeTorque / 3;
                    axleInfo.rightWheel.brakeTorque = maxBrakeTorque / 3;
                }
                
            }
            else
            {
                axleInfo.leftWheel.brakeTorque = 0;
                axleInfo.rightWheel.brakeTorque = 0;
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel); 
        }
    }
}