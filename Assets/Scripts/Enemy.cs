using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float enemyHealth = 100f;
    public float attackDamage = 25f;
    public float restTime = 5f;
    public bool dead = false;

    public bool isAttacking = false;
    private Player player;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        //player = FindObjectOfType<Player>();
        anim = this.transform.Find("girlEnemy").gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth <= 0 && !dead)
        {
            Die();
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && !dead)
        {
            player = collision.gameObject.GetComponent<Player>();

            Debug.Log("hit player collider");

            if (!isAttacking)
            {
                StartCoroutine(Attack());

            }
        }
    }

    IEnumerator Attack()
    {
        Debug.Log("attacking");
        player.health -= attackDamage;
        print(player.health);
        isAttacking = true;
        anim.SetBool("isAttacking", true);
        yield return new WaitForSeconds(restTime);
        anim.SetBool("isAttacking", false);
        isAttacking = false;
    }

    public void TakeDamage(float amount)
    {
        enemyHealth -= amount;
    }

    public void Die()
    {
        dead = true;
        //this.gameObject.SetActive(false);
        print("Enemy died.");
        anim.SetBool("isDead", true);
    }

}
