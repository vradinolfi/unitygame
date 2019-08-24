using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float walkSpeed = 3f;            // The speed that the player will walk at.
    public float runSpeed = 6f;            // The speed that the player will walk at.
    public float turnSpeed = 150f;            // The speed that the player will turn at.
    public bool isRunning = false;

    Animator anim;                      // Reference to the animator component.
    Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.

    void Awake()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");

        // Set up references.
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        // Store the input axes.
        float h = Input.GetAxisRaw("Horizontal") * Time.deltaTime * turnSpeed;
        float v = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;

        bool movingForward = false;

        if (Input.GetAxisRaw("Vertical") == 1)
        {

            if (movingForward == false)
            {
                // Call your event function here.
                movingForward = true;
            }

        } else
        {
            movingForward = false;
        }

        // Move the player around the scene.
        Move(h, v);

        if (movingForward && Input.GetButton("Run"))
        {

            isRunning = true;
            speed = runSpeed;
            //print("Running");

        }
        else
        {

            isRunning = false;
            speed = walkSpeed;
            //print("Not Running");

        }

        // Animate the player.
        Animating(h, v);
    }

    void Move(float h, float v)
    {
        playerRigidbody.transform.Rotate(0, h, 0);
        playerRigidbody.transform.Translate(0, 0, v);
        playerRigidbody.velocity = new Vector3(0, -2, 0);
    }

    void Animating(float h, float v)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
        bool walking = h != 0f || v != 0f;

        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsWalking", walking);
        anim.SetBool("IsRunning", isRunning);
    }
}