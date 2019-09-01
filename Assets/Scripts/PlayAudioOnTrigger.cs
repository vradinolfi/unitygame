using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnTrigger : MonoBehaviour
{
    public AudioSource myAudioSource;
    private GameObject player;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (myAudioSource.isPlaying)
            {

            } else
            {
                myAudioSource.Play();
            }
        }
    }
}
