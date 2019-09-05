using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;
    public float walkSpeed = 3f;            // The speed that the player will walk at.
    public float backSpeed = 1.5f;       // The speed that the player will backpedal at.
    public float runSpeed = 6f;             // The speed that the player will run at.
    public float turnSpeed = 150f;          // The speed that the player will turn at.
    public bool isRunning = false;
    public float maxHealth;
    public float poisonRate;
    public bool isPoisoned;
    public float rayDistance = 4f;
    public bool gamePaused = false;
    public GameObject pauseMenu;
    public GameObject inventory;

    public float health;

    private bool dead;
    private RaycastHit hit;
    private Ray ray;

    Animator anim;                      // Reference to the animator component.
    Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
    CapsuleCollider[] colliders;
    CapsuleCollider playerCollider;
    PlayerControls controls;
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.

    //private GameObject enemy;
    private Enemy enemyScript;

    void Awake()
    {
        controls = new PlayerControls();
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");

        // Set up references.
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        colliders = GetComponents<CapsuleCollider>();
        playerCollider = colliders[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        //enemy = GameObject.Find("Enemy1");
        //enemyScript = enemy.GetComponent<Enemy>();
    }

    void FixedUpdate()
    {
        // Store the input axes.
        float h = Input.GetAxisRaw("Horizontal") * Time.deltaTime * turnSpeed;
        float v = Input.GetAxisRaw("Vertical") * Time.deltaTime * speed;

        bool movingForward = false;
        bool movingBackward = false;

        if (Input.GetAxisRaw("Vertical") == 1)
        {

            if (movingForward == false)
            {
                // Call your event function here.
                movingForward = true;
            }

        }
        else if (Input.GetAxisRaw("Vertical") == -1)
        {
            movingForward = false;
            movingBackward = true;
        }
        else
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
        else if (movingBackward)
        {
            speed = backSpeed;
            //print("Walking backward");
        }
        else
        {

            isRunning = false;
            speed = walkSpeed;
            //print("Walking forward");

        }

        // Animate the player.
        Animating(h, v);

        ray = new Ray(transform.position + new Vector3(0f, playerCollider.center.y, 0f), transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, Color.red); // make sure gizmos are toggled on in viewport

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.distance < rayDistance)
            {
                //print("We hit something");
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    //print("Enemy sighted");
                }
            }
        }

        if (health <= 0 && !dead)
        {
            Die();
        }

        if (isPoisoned && !dead)
        {
            health -= poisonRate * Time.deltaTime;
        }

        if (Input.GetButton("Aim"))
        {
            anim.SetBool("IsAiming", true);
            //enemyScript.enemyHealth -= 25f;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.distance < rayDistance)
                {
                    //print("We hit something");
                    if (hit.collider.gameObject.tag == "Enemy")
                    {
                        print("Enemy locked");
                        //turnSpeed = 0;

                        Vector3 targetPostition = new Vector3(hit.transform.position.x,
                                        this.transform.position.y,
                                        hit.transform.position.z);
                        transform.LookAt(targetPostition);

                        if (Input.GetButtonDown("Submit"))
                        {
                            enemyScript = hit.collider.gameObject.GetComponent<Enemy>();
                            enemyScript.enemyHealth -= 25;
                            print("Shot fired!");
                        }

                    }
                }
            }
        } else
        {
            anim.SetBool("IsAiming", false);
        }

    }

    private void Update()
    {
        // Pause Game
        /*if (Input.GetButtonDown("Cancel"))
        {
            if (gamePaused == false)
            {
                Time.timeScale = 0;
                gamePaused = true;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                pauseMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                gamePaused = false;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                pauseMenu.SetActive(false);
            }
        }*/

        // Open Inventory
        if (Input.GetButtonDown("Inventory"))
        {
            if (gamePaused == false)
            {
                Time.timeScale = 0;
                gamePaused = true;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                inventory.SetActive(true);
            }
            else
            {

                Time.timeScale = 1;
                gamePaused = false;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                inventory.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            enemyScript = collision.gameObject.GetComponent<Enemy>();
            enemyScript.enemyHealth -= 25;
        }
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
        bool walking = h != 0f || v > 0f;
        bool walkingBackwards = h != 0f || v < 0f;

        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsWalking", walking);
        //anim.SetBool("IsWalkingBackwards", walkingBackwards);
        anim.SetBool("IsRunning", isRunning);
    }

    public void Heal(float healValue)
    {
        health += healValue;

        if(health > maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Cure()
    {
        isPoisoned = false;
    }

    public void Poison()
    {
        isPoisoned = true;
    }

    public void Die()
    {
        dead = true;
        print("You died.");
    }
}
