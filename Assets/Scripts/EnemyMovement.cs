using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{

    private NavMeshAgent nav;
    private Transform player;
    private Enemy enemy;

    public bool findPlayer;

    public float time1;
    public float time2;

    bool isWalking;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        enemy = GetComponent<Enemy>();

        //enemy.anim.StartPlayback();

    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.dead == false)
        {
            if (!enemy.isAttacking && findPlayer == true)
            {
                nav.SetDestination(player.position);

                enemy.anim.SetBool("isWalking", true);

                if (isWalking == true)
                {

                }
                else
                {
                    isWalking = true;
                    //StartCoroutine(Walk());   
                }


            }
            else
            {
                nav.SetDestination(this.transform.position);

                enemy.anim.SetBool("isWalking", false);
                //StopAllCoroutines();

            }
        }
    }

    /*IEnumerator Walk()
    {
        while (isWalking == true)
        {
            nav.SetDestination(player.position);

            Debug.Log("moving");

            //yield return new WaitForSecondsRealtime(time1);

            //nav.SetDestination(this.transform.position);

            //Debug.Log("paused");

            //yield return new WaitForSecondsRealtime(time2);

        }


    }*/

    void StartWalk()
    {
        //StartCoroutine(Walk());
    }
}
