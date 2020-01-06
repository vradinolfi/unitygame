using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOn : MonoBehaviour
{
    Player player;
    public AudioSource source;

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            source.Play();
        }
    }
}
