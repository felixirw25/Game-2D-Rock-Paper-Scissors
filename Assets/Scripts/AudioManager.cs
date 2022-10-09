using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SFXSlider;
    [SerializeField] Toggle BGMToggle;
    [SerializeField] Toggle SFXToggle;
    // public AudioSource musicSource;
    // public AudioSource soundSource;
    // public bool isMute
    // {
    //     get
    //     {
    //     int pref = PlayerPrefs.GetInt("mute", 0);
    //     return pref == 1 ? true : false;
    //     }
    //     set
    //     {
    //     if (musicSource != null) musicSource.mute = value;
    //     if (soundSource != null) soundSource.mute = value;
    //     PlayerPrefs.SetInt("mute", value ? 1 : 0);
    //     }
    // }
    //     public float musicVolume
    // {
    //     get
    //     {
    //     if (musicSource)
    //     {
    //         return musicSource.volume;
    //     }

    //     return PlayerPrefs.GetFloat("musicVolume", 1f);
    //     }

    //     set
    //     {
    //     if (musicSource != null) musicSource.volume = value;
    //     PlayerPrefs.SetFloat("musicVolume", value);
    //     }
    // }

    // void Awake()
    // {
    //     int mutePref = PlayerPrefs.GetInt("mute", 0);
    //     isMute = mutePref == 1 ? true : false;

    //     float musicVolumePref = PlayerPrefs.GetFloat("musicVolume", 1f);
    //     musicVolume = musicVolumePref;
    // }
    public void Start(){
        float vol;
        if(mixer.GetFloat("BGM_Volume", out vol)){
            BGMSlider.value = vol;
        }
        if(mixer.GetFloat("SFX_Volume", out vol)){
            SFXSlider.value = vol;
        }
    }
    public void BGMVolume(float newVol){
        mixer.SetFloat("BGM_Volume", newVol);
        Debug.Log("Volume BGM: "+ newVol);
    }
    public void SFXVolume(float newVol){
        mixer.SetFloat("SFX_Volume", newVol);
        Debug.Log("Volume SFX: "+ newVol);
    }
    public void BGMIsOn(){
        if(BGMToggle.isOn){
            Debug.Log("BGM Volume is On");
        }
        else{
            Debug.Log("BGM Volume is Muted");
        }
    }
    public void SFXIsOn(){
        if(SFXToggle.isOn){
            Debug.Log("SFX Volume is On");
        }
        else{
            Debug.Log("SFX Volume is Muted");
        }
    }
}
