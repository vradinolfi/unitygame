using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepSound : MonoBehaviour
{
    [Header("THIS OBJECT SHOULD BE ATTACHED TO PARENT")]
    [Header("BUT USE SEPARATE AUDIO SOURCE AS A CHILD AND ATTACH")]

    public AudioSource audioSource;
    public Animator anim;
    private float stepDelay;
    public float walkDelay;
    public float runDelay;
    public AudioClip defaultClip;
    public AudioClip defaultClip1;
    public AudioClip defaultClip2;
    public AudioClip defaultClip3;
    public AudioClip defaultClip4;
    private AudioClip currentClip;
    private bool couroutineOn;

    Player player;

    void Start()
    {

        anim = this.gameObject.GetComponent<Animator>();

        player = FindObjectOfType<Player>();

        couroutineOn = true;
        audioSource.clip = defaultClip;

        StartCoroutine(Walking());
    }

    IEnumerator Walking()
    {

        while (couroutineOn == true)
        {

            if (!player.isAiming && player.isWalking || player.isRunning || anim.GetCurrentAnimatorStateInfo(0).IsName("HumanoidRun") || anim.GetCurrentAnimatorStateInfo(0).IsName("HumanoidWalk") || anim.GetCurrentAnimatorStateInfo(0).IsName("Stealth"))
            {

                int rand = Random.Range(0, 4);
                if (rand == 0)
                {
                    currentClip = defaultClip;
                }
                else if (rand == 1)
                {
                    currentClip = defaultClip1;
                }
                else if (rand == 2)
                {
                    currentClip = defaultClip2;
                }
                else if (rand == 3)
                {
                    currentClip = defaultClip3;
                }
                else if (rand == 4)
                {
                    currentClip = defaultClip4;
                }

                audioSource.clip = currentClip;

                audioSource.Play();

            }
            else
            {

                audioSource.Stop();
            }


            if (anim.GetCurrentAnimatorStateInfo(0).IsName("HumanoidWalk") || player.isWalking && !player.isRunning)
            {
                stepDelay = walkDelay;
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).IsName("HumanoidRun") || player.isRunning)
            {
               stepDelay = runDelay;
            }

            yield return new WaitForSeconds(stepDelay);


        }
    }
}