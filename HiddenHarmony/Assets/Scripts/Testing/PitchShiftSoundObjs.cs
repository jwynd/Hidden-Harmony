using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PitchShiftSoundObjs : MonoBehaviour
{
    public AudioMixer soundObjPitch;
    public float pitch; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnTriggerEnter()
    {
        soundObjPitch.SetFloat("SoundObjPitch", pitch);
    }
}
