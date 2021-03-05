using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoudEffect : MonoBehaviour
{
    public AudioSource audioSource, music;
    public AudioClip jumpSound, coinSound, winSound, loseSound;


    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }
    public void PlayCoinSound()
    {
        audioSource.PlayOneShot(coinSound);
    }
    public void PlayWinSound()
    {
        audioSource.PlayOneShot(winSound);
    }
    public void PlayLoseSound()
    {
        audioSource.PlayOneShot(loseSound);
    }

}
