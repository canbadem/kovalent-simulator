using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;

    public void Play()
    {
        audioSource.Play();
    }

    public void setVolume(float f)
    {
        audioSource.volume = f;
    }
}
