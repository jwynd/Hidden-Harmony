using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// create this required component....
public class W_AudioPeer : MonoBehaviour {

    // NOTE: make this a 'static' float so we can access it from any other script.
    public static float[] spectrumData = new float[512];



    // Use this for initialization
    void Start () {

    }


    // Update is called once per frame
    void Update () {

        GetSpectrumAudioSource ();

    }


    void GetSpectrumAudioSource()
    {
        // this method computes the fft of the audio data, and then populates spectrumData with the spectrum data.
        AudioListener.GetSpectrumData (spectrumData, 0, FFTWindow.Hanning);
    }
}


