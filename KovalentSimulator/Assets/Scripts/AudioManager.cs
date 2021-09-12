using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource audioSource;

    [Header("Sounds")]
    public AudioClip popSound;
    public AudioClip correctSound;
    public AudioClip wrongSound;

    public void pop()
    {
        this.playClip(popSound);
    }

    public void correct()
    {
        this.playClip(correctSound);
    }

    public void wrong()
    {
        this.playClip(wrongSound);
    }

    public void playClip(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    
}
