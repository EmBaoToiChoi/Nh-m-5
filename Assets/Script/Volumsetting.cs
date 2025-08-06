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
        // Load âm lượng đã lưu, mặc định là 1 nếu chưa có
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);

        volumeSlider.value = savedVolume;
        audioSource.volume = savedVolume;

        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;

        // Lưu lại âm lượng vào PlayerPrefs
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save(); // Không bắt buộc, nhưng đảm bảo lưu ngay
    }
}
