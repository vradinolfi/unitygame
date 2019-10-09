using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{

    public Light lightningSource;
    public AudioSource thunderSource;
    public AudioClip thunder1;
    public AudioClip thunder2;
    public AudioClip thunder3;

    public float offDurationMin;
    public float offDurationMax;
    public float onDurationMin;
    public float onDurationMax;

    AudioClip currentClip;
    int flashes;

    void Start()
    {
        StartCoroutine(DoOnOff());
    }


    IEnumerator DoOnOff()
    {
        while (true)
        {
            flashes = Random.Range(1, 5);

            lightningSource.enabled = false;
            yield return new WaitForSeconds(Random.Range(offDurationMin, offDurationMax));

            thunderSource.Stop();

            for (int i = 0; i < flashes; i++)
            {
                Debug.Log("flash");
                lightningSource.enabled = true;
                yield return new WaitForSeconds(Random.Range(onDurationMin, onDurationMax));
                lightningSource.enabled = false;
                yield return new WaitForSeconds(Random.Range(onDurationMin, onDurationMax));
            }

            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                currentClip = thunder1;
            }
            else if (rand == 1)
            {
                currentClip = thunder2;
            }
            else if (rand == 2)
            {
                currentClip = thunder3;
            }

            thunderSource.clip = currentClip;
            thunderSource.Play();
        }
    }
}
