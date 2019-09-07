using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float enemyHealth = 100f;
    public bool dead = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth <= 0 && !dead)
        {
            Die();
        }
    }

    public void TakeDamage(float amount)
    {
        enemyHealth -= amount;
    }

    public void Die()
    {
        dead = true;
        this.gameObject.SetActive(false);
        print("Enemy died.");
    }
}
