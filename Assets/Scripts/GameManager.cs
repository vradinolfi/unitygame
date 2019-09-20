using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool gamePaused;
    
    void Start()
    {

        // stops console warning about multiple audio listeners but i could see it causing issues later on maybe
        foreach (Camera camera in Camera.allCameras)
        {
            camera.GetComponent<AudioListener>().enabled = false;
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
