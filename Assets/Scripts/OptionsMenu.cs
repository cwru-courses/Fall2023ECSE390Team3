using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start(){
        if(PlayerPrefs.HasKey("musicVolume") || PlayerPrefs.HasKey("sfxVolume")){
            LoadVolume();
        }
        else{
            SetMusicVolume();
            SetSFXVolume();
        }
    }

    public void SetMusicVolume(){
        float volume = 1f;
        if(musicSlider){
            volume = musicSlider.value;
        }
        else if(PlayerPrefs.HasKey("musicVolume")){
            volume = PlayerPrefs.GetFloat("musicVolume");
        }
        audioMixer.SetFloat("music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSFXVolume(){
        float volume = 1f;
        if(sfxSlider){
            volume = sfxSlider.value;
        }
        else if(PlayerPrefs.HasKey("sfxVolume")){
            volume = PlayerPrefs.GetFloat("sfxVolume");
        }
        audioMixer.SetFloat("sfx", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    public void LoadVolume(){
        if(musicSlider){
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");

        }
        if(sfxSlider) {
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        }
        SetMusicVolume();
        SetSFXVolume();
    }

}
