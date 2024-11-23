using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider volumeSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicvolume"))
        {
            LoadVolume();
        }
        else
        {
            SetVolume();
        }
        
    }

    public void SetVolume()
    {
        float volume = volumeSlider.value;
        myMixer.SetFloat("volume", MathF.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicvolume",volume);
    }

    private void LoadVolume()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicvolume");
        
        SetVolume();
    }
}
