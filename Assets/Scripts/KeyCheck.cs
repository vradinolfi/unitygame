using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCheck : MonoBehaviour
{
    public Animation door;

    public GameObject player;

    void OnTriggerStay()
    {
        
        if (Input.GetButton("Interact"))
        {
            //if (player.GetComponentInChildren<Item>()) {
                door.Play();
            //}
        }

    }
}
