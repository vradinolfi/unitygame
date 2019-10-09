using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainAudioManager : MonoBehaviour
{
    public AudioSource rainSource;

    public AudioClip outdoorSound;
    public AudioClip indoorSound;

    public float fadeTime;

    bool check;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        rainSource.clip = outdoorSound;
        rainSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isIndoors != check)
        {
            check = player.isIndoors;

            

            if (!player.isIndoors)
            {
                StartCoroutine(FadeOut(rainSource, fadeTime, outdoorSound));
                //rainSource.clip = outdoorSound;
                //rainSource.Play();
            }
            else
            {
                StartCoroutine(FadeOut(rainSource, fadeTime, indoorSound));
                //rainSource.clip = indoorSound;
                //rainSource.Play();
            }
        }
    }

    IEnumerator FadeOut(AudioSource audioSource, float FadeTime, AudioClip newClip)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
            Debug.Log(audioSource.volume);
            yield return null;
        }

        audioSource.Stop();
        rainSource.clip = newClip;
        audioSource.volume = .003f;
        audioSource.Play();
        while (audioSource.volume < startVolume)
        {
            audioSource.volume += audioSource.volume * Time.deltaTime / FadeTime;
            Debug.Log(audioSource.volume);
            yield return null;
        }


        //audioSource.volume = startVolume;
    }
}
