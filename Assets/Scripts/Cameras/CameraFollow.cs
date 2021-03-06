﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 1f;
    public Vector3 desiredPosition;

    public float distance;

    public bool enableCollision;

    void OnEnable()
    {
        transform.position = target.position + offset;
    }

    void FixedUpdate()
    {

        desiredPosition = target.position + offset;

        if (enableCollision)
        {
            RaycastHit hit;

            if (Physics.Linecast(target.position, desiredPosition, out hit))
            {
                desiredPosition.x = hit.point.x - .1f;
                desiredPosition.z = hit.point.z - .1f;
            }
        }

        Vector3 smoothedPosition = cubeBezier3(
                transform.position,
                transform.position,
                desiredPosition,
                desiredPosition,
                smoothSpeed * Time.deltaTime
                );

        transform.position = smoothedPosition;

    }

    public static Vector3 cubeBezier3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float r = 1f - t;
        float f0 = r * r * r;
        float f1 = r * r * t * 3;
        float f2 = r * t * t * 3;
        float f3 = t * t * t;

        return new Vector3(
            f0 * p0.x + f1 * p1.x + f2 * p2.x + f3 * p3.x,
            f0 * p0.y + f1 * p1.y + f2 * p2.y + f3 * p3.y,
            f0 * p0.z + f1 * p1.z + f2 * p2.z + f3 * p3.z
        );
    }
}
