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

            foreach(Camera camera in Camera.allCameras)
            {
                //camera.transform.gameObject.SetActive(false);
                camera.enabled = false;
                camera.GetComponent<AudioListener>().enabled = false;
            }

            //targetCamera.transform.gameObject.SetActive(true);
            targetCamera.enabled = true;
            targetCamera.GetComponent<AudioListener>().enabled = true;

        }

    }

}
