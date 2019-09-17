using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    public Transform target;
    public int xOffset = 0;
    public int yOffset = 0;

    void FixedUpdate()
    {
        transform.LookAt(target);
        transform.rotation *= Quaternion.Euler(xOffset, yOffset, 0);
    }
}
