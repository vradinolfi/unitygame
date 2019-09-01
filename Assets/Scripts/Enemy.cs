using System.Collections;
using System.Collections.Generic;
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

    public void Die()
    {
        dead = true;
        print("Enemy died.");
    }
}
