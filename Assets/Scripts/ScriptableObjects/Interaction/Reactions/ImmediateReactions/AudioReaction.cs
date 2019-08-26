using UnityEngine;

public class AudioReaction : Reaction
{
    public AudioSource audioSource;
    public AudioClip audioClip;
    public float delay;


    protected override void ImmediateReaction()
    {
        audioSource.clip = audioClip;
        audioSource.PlayDelayed(delay);
    }
}