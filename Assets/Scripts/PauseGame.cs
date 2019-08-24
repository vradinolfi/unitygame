using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{

    public bool gamePaused = false;
    public GameObject pauseMenu;

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Cancel"))
        {
            if (gamePaused == false)
            {
                Time.timeScale = 0;
                gamePaused = true;
                Cursor.visible = true;
                pauseMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                gamePaused = false;
                Cursor.visible = false;
                pauseMenu.SetActive(false);
            }
        }
        
    }
}
