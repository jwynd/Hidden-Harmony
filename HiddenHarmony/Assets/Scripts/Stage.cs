using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public int[] halfBeats;
    public int[] pitches;
    public GameObject[] crystals;

    void Start(){
        if(halfBeats.Length != pitches.Length) throw new System.ArgumentException("Beats and pitches must be of equal length");
        foreach(float halfBeat in halfBeats){
            if (halfBeat <= 0) throw new System.ArgumentException("All beats must be greater than zero");
        }
        foreach(int pitch in pitches){
            if(pitch > 7 || pitch < 0) throw new System.ArgumentException("All pitches must be between 0 and 7 inclusive");
        }
    }
}
