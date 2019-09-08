using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSound : MonoBehaviour
{
    public void PlaySound()
    {
        this.GetComponent<AudioSource>().Play();
    }
}
