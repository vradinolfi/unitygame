using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableObject : MonoBehaviour
{
    private bool isInRange = false;
    Player player;
    public float force;
    RaycastHit hit;
    Quaternion targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
    }

    void OnTriggerStay(Collider c)
    {
        // force is how forcefully we will push the player away from the enemy.
        //float force = 3;

        //this.GetComponent<Rigidbody>().velocity = player.GetComponent<Rigidbody>().velocity;

        // If the object we hit is the enemy
        if (c.gameObject.CompareTag("Player"))
        {
            if (Physics.Raycast(player.transform.position + new Vector3(0f, player.GetComponent<CharacterController>().center.y, 0f), player.transform.forward, out hit))
            {
                if (hit.collider.gameObject.tag == "Pushable")
                {
                    player.isPushing = true;
                    this.GetComponent<Rigidbody>().AddForce(player.transform.forward*5);
                    //= player.GetComponent<Rigidbody>().velocity;
                    Debug.Log("player colliding");

                    /*
                    if (player.transform.rotation.y > 0.35 && player.transform.rotation.y < 0.85)
                    {
                        player.transform.Rotate(0, 90f, 0);
                    }
                    else if (player.transform.rotation.y > 0.85 || player.transform.rotation.y < -0.85)
                    {
                        player.transform.Rotate(0, 180f, 0);
                    } else if (player.transform.rotation.y < 0.35 && player.transform.rotation.y > -0.35)
                    {
                        player.transform.Rotate(0, 0f, 0);
                    }
                    else if (player.transform.rotation.y < -0.35 && player.transform.rotation.y > -0.85)
                    {
                        player.transform.Rotate(0, 270f, 0);
                    }*/

                    //Vector3 currentLook = player.transform.rotation * Vector3.up;// or you can use transform.up

                    // flatten direction to one world axis
                    //if (Mathf.Abs(currentLook.x) > Mathf.Abs(currentLook.y))
                    //    currentLook.y = 0;
                    //else
                    //    currentLook.x = 0;

                    //currentLook = currentLook.normalized;


                    //now convert the new look direction back to Quaternion
                    //targetRotation = Quaternion.LookRotation(Vector3.forward, currentLook);

                    //player.transform.rotation = targetRotation;


                    // Calculate Angle Between the collision point and the player
                    //Vector3 dir = c.contacts[0].point - transform.position;
                    // We then get the opposite (-Vector3) and normalize it
                    //dir = -dir.normalized;
                    // And finally we add force in the direction of dir and multiply it by force. 
                    // This will push back the player
                    //GetComponent<Rigidbody>().AddForce(dir * force);
                }
            }
        }
    }

    void OnTriggerExit(Collider c)
    {
        //this.GetComponent<Rigidbody>().AddForce(0,0,0);
        if (c.gameObject.CompareTag("Player")) {
            player.isPushing = false;
        }
    }
}
