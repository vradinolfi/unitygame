using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotZone : MonoBehaviour
{

    //public Shot targetShot;

    public Camera targetCamera;

    void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            //targetShot.CutToShot();

            foreach(Camera cam in Camera.allCameras)
            {
                //camera.transform.gameObject.SetActive(false);
                cam.enabled = false;
                cam.GetComponent<AudioListener>().enabled = false;
            }

            //targetCamera.transform.gameObject.SetActive(true);
            targetCamera.enabled = true;
            targetCamera.GetComponent<AudioListener>().enabled = true;

        }

    }

}
