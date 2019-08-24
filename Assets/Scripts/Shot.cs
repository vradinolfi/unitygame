using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{

    public Vector3 focalPoint;

    public void CutToShot()
    {
        transform.LookAt(focalPoint);
        Camera.main.transform.localPosition = transform.position;
        Camera.main.transform.localRotation = transform.rotation;
    }

    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            CutToShot();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, focalPoint);
    }

}
