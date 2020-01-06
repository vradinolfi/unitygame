using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private bool isInRange = false;
    Player player;
    public float force = 5f;
    RaycastHit hit;
    Quaternion targetRotation;

    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void OnTriggerStay(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            if (Physics.Raycast(player.transform.position + new Vector3(0f, player.GetComponent<CharacterController>().center.y, 0f), player.transform.forward, out hit))
            {
                if (hit.collider.gameObject.tag == "Pushable")
                {
                    player.isPushing = true;
                    this.GetComponent<Rigidbody>().AddForce(player.transform.forward * force);
                    //Debug.Log("player colliding");

                }
            }
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.gameObject.CompareTag("Player")) {
            player.isPushing = false;
        }
    }
}
