using UnityEngine;
using UnityEngine.UI;

public class SFXVolumeManager : MonoBehaviour
{
    public Slider sfxSlider;
    public AudioSource[] sfxAudioSources; // Các AudioSource phát SFX trong scene này

    private void Start()
    {
        // Lấy volume đã lưu hoặc mặc định 1.0f
        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        sfxSlider.value = savedVolume;
        ApplyVolume(savedVolume);

        // Bắt sự kiện thay đổi
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    private void OnSFXVolumeChanged(float value)
    {
        ApplyVolume(value);
        PlayerPrefs.SetFloat("SFXVolume", value); // Lưu lại
    }

    private void ApplyVolume(float value)
    {
        foreach (AudioSource audioSource in sfxAudioSources)
        {
            audioSource.volume = value;
        }
    }
}
