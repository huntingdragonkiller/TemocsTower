using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;
    
    public void SetMasterVolume(float level){
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
    }
    public void SetSFXVolume(float level){
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(level) * 20f);
    }
    public void SetMenuSFXVolume(float level){
        audioMixer.SetFloat("MenuSFXVolume", Mathf.Log10(level) * 20f);
    }
    public void SetMusicVolume(float level){
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
    }
    public void SetAmbienceVolume(float level){
        audioMixer.SetFloat("AmbienceVolume", Mathf.Log10(level) * 20f);
    }
}
