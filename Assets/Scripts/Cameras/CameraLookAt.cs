using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookAt : MonoBehaviour
{
    public Transform target;
    public int xOffset = 0;

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
        transform.rotation *= Quaternion.Euler(xOffset, 0, 0);
    }
}
