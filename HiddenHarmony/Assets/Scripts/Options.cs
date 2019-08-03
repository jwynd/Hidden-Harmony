using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{

    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private AudioMixer[] mixers;
    private Resolution[] resolutions;


    void Start(){
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentIndex = 0;

        foreach(Resolution res in resolutions){
            string option = res.width + " x " + res.height;
            options.Add(option);

            if(res.width == Screen.currentResolution.width &&
               res.height == Screen.currentResolution.height){
                   currentIndex = options.Count - 1;
               }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentIndex;
        resolutionDropdown.RefreshShownValue();

    }

    public void SetQuality (int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen (bool isFullscreen){
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution (int resolutionIndex){
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetMusicVolume (float volume){
        foreach(AudioMixer mixer in mixers){
            mixer.SetFloat("MusicVolume", volume);
        }
    }

    public void SetSFXVolume (float volume){
        foreach(AudioMixer mixer in mixers){
            mixer.SetFloat("SFXVolume", volume);
        }
    }

    public void SetAmbianceVolume (float volume){
        foreach(AudioMixer mixer in mixers){
            mixer.SetFloat("AmbianceVolume", volume);
        }
    }

    public void SetMasterVolume (float volume){
        foreach(AudioMixer mixer in mixers){
            mixer.SetFloat("MasterVolume", volume);
        }
    }
}
