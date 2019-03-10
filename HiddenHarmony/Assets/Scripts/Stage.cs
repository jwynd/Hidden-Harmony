using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public float[] beats;
    public int[] pitches;

    void Start(){
        if(beats.Length != pitches.Length) throw new System.ArgumentException("Beats and pitches must be of equal length");
        foreach(float beat in beats){
            if (beat <= 0) throw new System.ArgumentException("All beats must be greater than zero");
        }
        foreach(int pitch in pitches){
            if(pitch > 8 || pitch < 0) throw new System.ArgumentException("All pitches must be between 0 and 8 inclusive");
        }
    }
}
