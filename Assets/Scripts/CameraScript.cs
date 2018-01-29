using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject cameraObject;

    public Vector3 distanceFromPlayer;
    public float rotationSpeed;
    public float translationSpeed;

    void Start()
    {
        cameraObject.transform.position = transform.position - distanceFromPlayer;
    }

    public void UpdateCamera()
    {
        float rotationX = Mathf.LerpAngle(cameraObject.transform.eulerAngles.x, transform.eulerAngles.x,
            rotationSpeed * Time.deltaTime);
        float rotationY = Mathf.LerpAngle(cameraObject.transform.eulerAngles.y, transform.eulerAngles.y,
            rotationSpeed * Time.deltaTime);
        float rotationZ = Mathf.LerpAngle(cameraObject.transform.eulerAngles.z, transform.eulerAngles.z,
            rotationSpeed * Time.deltaTime);
        
        cameraObject.transform.eulerAngles = new Vector3(rotationX, rotationY, rotationZ);

        Vector3 destination = transform.position - transform.forward * distanceFromPlayer.z;
        destination -= transform.up * distanceFromPlayer.y;

        cameraObject.transform.position = Vector3.Lerp(cameraObject.transform.position, destination, translationSpeed * Time.deltaTime);
    }
}