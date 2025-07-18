using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Volumsetting : MonoBehaviour
{
    public Slider volumeSlider;     
    public AudioSource audioSource; 

    void Start()
    {
        
        volumeSlider.value = audioSource.volume;

        
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    
    public void SetVolume(float volume)
    {
        audioSource.volume = volume; 
    }

}
