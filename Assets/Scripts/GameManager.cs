using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool gamePaused = false;
    public bool playerBusy = false;
    
    void Start()
    {

        // stops console warning about multiple audio listeners but i could see it causing issues later on maybe
        foreach (Camera cam in Camera.allCameras)
        {
            cam.GetComponent<AudioListener>().enabled = false;

            //Debug.Log(cam.gameObject.name);
        }

    }

    void Update()
    {
        
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        gamePaused = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        gamePaused = false;
    }
}
