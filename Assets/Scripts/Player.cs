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
    public float gravity = 20f;
    public float rayDistance = 4f;
    public float maxHealth;
    public float curSpeed;
    public float animSpeed;
    [Space]
    public bool isWalking = false;
    public bool isRunning = false;
    public float poisonRate;
    public bool isPoisoned;
    public bool isAiming = false;

    public bool gamePaused = false;
    [Space]
    public GameObject pauseMenu;
    public GameObject inventoryUI;
    public InventoryObject inventory;

    [Space]

    public float health;

    private bool dead;
    private RaycastHit hit;
    private Ray ray;

    Item item;
    public Animator anim;                      // Reference to the animator component.
    //Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
    CharacterController playerController;
    CapsuleCollider[] colliders;
    CapsuleCollider playerCollider;
    int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    GameObject girl;
    //private GameObject enemy;
    private Enemy enemyScript;

    void Awake()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");

        girl = GameObject.Find("/Player/girl2@Idle2");
        // Set up references.
        anim = girl.GetComponent<Animator>();
        playerController = GetComponent<CharacterController>();
        colliders = GetComponents<CapsuleCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
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

        if (movingForward && Input.GetButton("Run"))
        {

            isRunning = true;
            speed = runSpeed;

        }
        else if (movingBackward)
        {
            speed = backSpeed;
        }
        else
        {

            isRunning = false;
            speed = walkSpeed;

        }
        
        if (isAiming)
        {
            speed = 0;
            isRunning = false;
        }

        // Move the player around the scene.
        Move(h, v);

        // Animate the player.
        Animating(h, v);

        if (health <= 0 && !dead)
        {
            Die();
        }

        if (isPoisoned && !dead)
        {
            health -= poisonRate * Time.deltaTime;
        }

    }

    private void Update()
    {

        // Open Inventory
        if (Input.GetButtonDown("Inventory"))
        {
            if (gamePaused == false)
            {
                Time.timeScale = 0;
                gamePaused = true;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                inventoryUI.SetActive(true);
            }
            else
            {

                Time.timeScale = 1;
                gamePaused = false;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                inventoryUI.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            inventory.Save();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            inventory.Load();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {

    }


    /*
    public void OnTriggerEnter(Collider other)
    {
        item = other.GetComponent<Item>();

        if (item)
        {
            //inventory.AddItem(item.item, 1);
            //Destroy(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (item)
        {
            item = null;
        }
    }*/

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }



    void Move(float h, float v)
    {

            
        Vector3 move = new Vector3(0, 0, v);

        move = transform.TransformDirection(move);

        move.y -= gravity * Time.deltaTime;

        playerController.Move(move);

        //Debug.Log(move);
        
        Vector3 turn = new Vector3(0, h, 0);

        transform.Rotate(turn);

        //Debug.Log(turn);

    }

    void Animating(float h, float v)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
        bool walking = h != 0f || v > 0f;
        bool walkingBackwards = h != 0f || v < 0f;


        isWalking = walking;
        // Tell the animator whether or not the player is walking.
        if (isAiming)
        {
            walking = false;
            animSpeed = 0f;
        }

        if (walking)
        {
            animSpeed = 0.25f;
        }

        if (isRunning)
        {
            animSpeed = 1f;
        }

        if (!isRunning && !walking)
        {
            animSpeed = 0f;
        }

        curSpeed = Mathf.Lerp(curSpeed, animSpeed, runSpeed * Time.deltaTime);

        anim.SetFloat("Speed", curSpeed);

        //anim.SetBool("IsWalking", walking);
        //anim.SetBool("IsWalkingBackwards", walkingBackwards);
        //anim.SetBool("IsRunning", isRunning);
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
