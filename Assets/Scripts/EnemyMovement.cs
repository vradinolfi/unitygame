using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{

    private NavMeshAgent nav;
    private Transform player;
    private Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!enemy.isAttacking)
        {
            nav.SetDestination(player.position);
        }
        else
        {
            nav.SetDestination(this.transform.position);
        }
    }
}
