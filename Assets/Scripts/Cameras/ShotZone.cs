using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotZone : MonoBehaviour
{

    //public Shot targetShot;

    public Camera targetCamera;

    void Start()
    {



    }

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            //targetShot.CutToShot();

            foreach(Camera camera in Camera.allCameras)
            {
                camera.enabled = false;
                camera.GetComponent<AudioListener>().enabled = false;
            }

            targetCamera.enabled = true;
            targetCamera.GetComponent<AudioListener>().enabled = true;

        }

    }

}
