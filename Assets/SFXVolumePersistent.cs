using UnityEngine;

public class SFXVolumePersistent : MonoBehaviour
{
    public AudioSource[] sfxSources;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        foreach (AudioSource source in sfxSources)
        {
            source.volume = savedVolume;
        }
    }
}
