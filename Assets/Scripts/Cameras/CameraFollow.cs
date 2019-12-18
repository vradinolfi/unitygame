using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothSpeed = 1f;
    public Vector3 desiredPosition;

    void OnEnable()
    {
        transform.position = target.position + offset;
    }

    void FixedUpdate()
    {
        //if (this.gameObject.GetComponent<Camera>().enabled)
        //{
            /*
            RaycastHit hit;

            if (Physics.Linecast(target.position, transform.position, out hit))
            {
                Debug.Log("blocked");
                desiredPosition = hit.point;
            }
            else
            {*/
                desiredPosition = target.position + offset;
                //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            //}

            Vector3 smoothedPosition = cubeBezier3(
                transform.position,
                transform.position,
                desiredPosition,
                desiredPosition,
                smoothSpeed * Time.deltaTime
                );

            transform.position = smoothedPosition;
        //}

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
