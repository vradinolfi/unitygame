using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCheck : MonoBehaviour
{
    public Animation door;

    // Update is called once per frame
    void OnTriggerStay()
    {
        
        if (Input.GetButton("Interact"))
        {
            door.Play();
        }

    }
}
