using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float maxHealth;
    public float poisonRate;
    public bool isPoisoned;

    private float health;
    private bool dead;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && !dead)
        {
            Die();
        }

        if (isPoisoned && !dead)
        {
            health -= poisonRate * Time.deltaTime;
        }

        print(health);
    }

    public void Die()
    {
        dead = true;
        print("You died.");
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
}
