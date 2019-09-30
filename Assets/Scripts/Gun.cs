using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public float damage = 20f;
    public float range = 200f;
    public float fireRate = 15f;

    private float nextTimeToFire = 0f;

    public int maxAmmo = 15;
    private int currentAmmo;
    private int availableAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public bool equipPistol;

    public Player player;
    public ParticleSystem muzzleFlash;
    public AudioSource shotSound;

    public AudioClip shotSound1;
    public AudioClip shotSound2;
    public AudioClip shotSound3;
    public AudioClip shotSound4;
    public AudioClip reloadSound;
    public AudioClip dryfireSound;

    private AudioClip currentClip;
    private CharacterController playerController;
    private Animator anim;

    RaycastHit hit;
    Ray ray;

    private void Start()
    {

        playerController = player.GetComponent<CharacterController>();
        anim = player.GetComponent<Animator>();

        //currentAmmo = maxAmmo;
        currentAmmo = 0;
    }

    private void OnEnable()
    {
        isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {

        for (int i = 0; i < player.inventory.Container.Count; i++)
        {
            if (player.inventory.Container[i].item.name == "Beretta92FS")
            {
                equipPistol = true;
            }

            if (player.inventory.Container[i].item.name == "9mm")
            {
                availableAmmo = player.inventory.Container[i].amount;
            } else
            {
                availableAmmo = 0;
            }
        }

        if (equipPistol == true)
        {

            if (Input.GetButton("Aim"))
            {
                Aim();
                anim.SetBool("IsAiming", true);
                player.isAiming = true;
            }
            else
            {
                anim.SetBool("IsAiming", false);
                player.isAiming = false;
            }

            if (isReloading)
            {
                return;
            }

            if (Input.GetButton("Aim") && Input.GetButtonDown("Submit") && Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Shoot();
            }

        }

        //ray = new Ray(player.transform.position + new Vector3(0f, playerCollider.center.y, 0f), player.transform.forward);

        //Debug.DrawRay(ray.origin, ray.direction * range, Color.red); // make sure gizmos are toggled on in viewport

    }

    void Aim()
    {
        if (Physics.Raycast(player.transform.position + new Vector3(0f, playerController.center.y, 0f), player.transform.forward, out hit, range))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {
                //print("Enemy locked");

                Vector3 targetPostition = new Vector3(hit.transform.position.x,
                                player.transform.position.y,
                                hit.transform.position.z);
                player.transform.LookAt(targetPostition);

            }
        }
    }

    void Shoot()
    {
        
        if (currentAmmo <= 0)
        {
            if (availableAmmo > 0)
            {
                StartCoroutine(Reload());
                return;
            }
            else
            {
                Debug.Log("Out of ammo.");
                shotSound.clip = dryfireSound;

                shotSound.Play();
            }
        }

        if (currentAmmo > 0)
        {

            currentAmmo--;

            muzzleFlash.Play();

            int rand = Random.Range(0, 3);
            if (rand == 0)
            {
                currentClip = shotSound1;
            }
            else if (rand == 1)
            {
                currentClip = shotSound2;
            }
            else if (rand == 2)
            {
                currentClip = shotSound3;
            }
            else if (rand == 3)
            {
                currentClip = shotSound4;
            }

            shotSound.clip = currentClip;

            shotSound.Play();

            if (Physics.Raycast(player.transform.position + new Vector3(0f, playerController.center.y, 0f), player.transform.forward, out hit, range))
            {
                //Debug.Log(hit.transform.name);

                Enemy enemy = hit.transform.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }

        }

        Debug.Log(currentAmmo);
        Debug.Log(availableAmmo);

    }

    IEnumerator Reload()
    {
        Debug.Log("Reloading");

        isReloading = true;

        shotSound.clip = reloadSound;

        shotSound.Play();

        yield return new WaitForSeconds(reloadTime);

        if (availableAmmo - maxAmmo < 0)
        {
            currentAmmo = availableAmmo;
            availableAmmo = 0;

        }
        else
        {
            availableAmmo -= 15;
            currentAmmo = maxAmmo;
        }

        for (int i = 0; i < player.inventory.Container.Count; i++)
        {
            if (player.inventory.Container[i].item.name == "9mm")
            {
                player.inventory.Container[i].amount = availableAmmo;
            }
        }

        isReloading = false;
    }
}
