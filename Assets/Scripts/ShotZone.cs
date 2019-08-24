using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotZone : MonoBehaviour
{

    public Shot targetShot;

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            targetShot.CutToShot();
        }

    }

}
