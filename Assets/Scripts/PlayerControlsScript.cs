using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class PlayerControlsScript : MonoBehaviour
{
    public string rollAxis;
    public string pitchAxis;
    public string yawAxis;
    public string accelerationAxis;

    public float drag;

    public float rollAcceleration;
    public float pitchAcceleration;
    public float yawAcceleration;

    public float maxRoll;
    public float maxPitch;
    public float maxYaw;

    public float maxRollAngle;
    public float pitchBlockAngle;

    public float minSpeed;
    public float maxSpeed;
    public float acceleration;
    public float standardSpeed; // Will always try to return to this value

    public GameObject explosionObject;
    
    public float hitPoints;
    float currentHitPoints;
    
    float currentRoll;
    float currentPitch;
    float currentYaw;
    float currentSpeed;
    
    void Start()
    {
        currentSpeed = standardSpeed;
        currentHitPoints = hitPoints;
    }

    void Update()
    {
        float inputRoll = Input.GetAxis(rollAxis);
        float inputPitch = Input.GetAxis(pitchAxis);
        float inputYaw = Input.GetAxis(yawAxis);
        float inputAcceleration = Input.GetAxis(accelerationAxis);

        if (Mathf.Abs(inputAcceleration) > 0.03f)
        {
            currentSpeed += acceleration * inputAcceleration * Time.deltaTime;
        }
        else
        {
            if (currentSpeed > standardSpeed) currentSpeed -= (acceleration * Time.deltaTime) / 4.0f;
            if (currentSpeed < standardSpeed) currentSpeed += (acceleration * Time.deltaTime) / 4.0f;
        }
        
        if (Mathf.Abs(inputRoll) > 0.03f)
        {
            currentRoll += rollAcceleration * inputRoll * Time.deltaTime;
        }
        else
        {
            if (currentRoll > 0.0f) currentRoll -= (rollAcceleration * Time.deltaTime) / 1.5f;
            if (currentRoll < 0.0f) currentRoll += (rollAcceleration * Time.deltaTime) / 1.5f;
        }

        if (((transform.eulerAngles.z > pitchBlockAngle && transform.eulerAngles.z <= 180.0f) ||
             (transform.eulerAngles.z < 360.0f - pitchBlockAngle && transform.eulerAngles.z > 180.0f)) &&
            inputPitch > 0.1f)
        {
            inputPitch = 0.1f;
        }
        
        if (Mathf.Abs(inputPitch) > 0.03f)
        {
            currentPitch += pitchAcceleration * inputPitch * Time.deltaTime;
        }
        else
        {
            if (currentPitch > 0.0f) currentPitch -= (pitchAcceleration * Time.deltaTime) / 1.5f;
            if (currentPitch < 0.0f) currentPitch += (pitchAcceleration * Time.deltaTime) / 1.5f;
        }
        
        if (Mathf.Abs(inputYaw) > 0.03f)
        {
            currentYaw += yawAcceleration * inputYaw * Time.deltaTime;
        }
        else
        {
            if (currentYaw > 0.0f) currentYaw -= (yawAcceleration * Time.deltaTime) / 2.0f;
            if (currentYaw < 0.0f) currentYaw += (yawAcceleration * Time.deltaTime) / 2.0f;
        }
        
        if (currentSpeed > maxSpeed) currentSpeed = maxSpeed;
        if (currentSpeed < minSpeed) currentSpeed = minSpeed;
        
        if (currentRoll > maxRoll) currentRoll = maxRoll;
        if (currentRoll < -maxRoll) currentRoll = -maxRoll;
        
        if (currentPitch > maxPitch) currentPitch = maxPitch;
        if (currentPitch < -maxPitch) currentPitch = -maxPitch;
        
        if (currentYaw > maxYaw) currentYaw = maxYaw;
        if (currentYaw < -maxYaw) currentYaw = -maxYaw;
        
        if (transform.eulerAngles.z > maxRollAngle && transform.eulerAngles.z <= 180.0f)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, maxRollAngle);
        if (transform.eulerAngles.z < 360.0f-maxRollAngle && transform.eulerAngles.z > 180.0f)
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -maxRollAngle);
        
        Vector3 newRotation = new Vector3(currentPitch, currentYaw, currentRoll);
        newRotation *= Time.deltaTime;
        transform.Rotate(newRotation);

        Vector3 newTranslation = new Vector3(0.0f, 0.0f, currentSpeed);

        newTranslation *= Time.deltaTime;
        transform.Translate(newTranslation);
        
        GetComponent<CameraScript>().UpdateCamera();
    }
    
    public void GetDamage(int damage)
    {
        currentHitPoints -= damage;

        if (currentHitPoints <= 0)
        {
            Instantiate(explosionObject, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}