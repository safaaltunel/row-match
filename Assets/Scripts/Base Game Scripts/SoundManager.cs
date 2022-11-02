using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public AudioSource popSound;

    public void PlayPopSound()
    {
        if((PlayerPrefs.HasKey("Sound") && PlayerPrefs.GetInt("Sound") == 1) || !PlayerPrefs.HasKey("Sound"))
            popSound.Play();
    }
}
