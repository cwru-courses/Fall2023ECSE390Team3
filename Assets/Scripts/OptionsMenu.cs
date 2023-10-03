using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;

    private void Start(){
        SetVolume();
    }

    public void SetVolume(){
        float volume = musicSlider.value;
        Debug.Log(volume);
        audioMixer.SetFloat("volume", Mathf.Log10(volume)*20);
    }
}
