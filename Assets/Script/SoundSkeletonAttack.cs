using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSkeletonAttack : MonoBehaviour
{
     public AudioClip attackClip;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
     public void PlayAttackSound()
    {
        if (attackClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(attackClip);
        }
    }
}
