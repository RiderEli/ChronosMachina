using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mainAudio;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sFXSlider;

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        mainAudio.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume()
    {
        float volume = sFXSlider.value;
        mainAudio.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
