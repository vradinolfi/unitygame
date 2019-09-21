using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    public Transform target;
    public float xOffset = 0;
    public float yOffset = 0;
    public float smoothSpeed = 2f;

    Vector3 offset;

    void FixedUpdate()
    {
        offset.Set(xOffset, yOffset, 0);

        var targetRotation = Quaternion.LookRotation((target.transform.position + offset) - transform.position);

        // Smoothly rotate towards the target
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);

        // Linear rotation
        //transform.LookAt(target);
        //transform.rotation *= Quaternion.Euler(xOffset, yOffset, 0);
        //transform.rotation *= Quaternion.Euler(xOffset, yOffset, 0);

    }
}
