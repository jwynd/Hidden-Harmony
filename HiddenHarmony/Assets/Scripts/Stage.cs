using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public int[] halfBeats;
    public int[] pitches;
    [SerializeField][Tooltip("Places an offset on when the crystal lights up. Must be between 0 and the number of notes - 1")]
    private int crystalOffset;
    [HideInInspector] public GameObject[] crystals;

    void Start(){
        if(halfBeats.Length != pitches.Length) throw new System.ArgumentException("Beats and pitches must be of equal length");
        foreach(float halfBeat in halfBeats){
            if (halfBeat <= 0) throw new System.ArgumentException("All beats must be greater than zero");
        }
        int[] temp = new int[pitches.Length];
        for(int i = 0; i < pitches.Length; i++){
            temp[i] = pitches[i];
        }
        for(int i = 0; i < pitches.Length; i++){
            if(i == pitches.Length-1){pitches[i]=temp[0];}
            else{pitches[i] = temp[i+1];}
        }
        /*foreach(int pitch in pitches){
            if(pitch > 7 || pitch < 0) throw new System.ArgumentException("All pitches must be between 0 and 7 inclusive");
        }*/
        int index;
        crystals = new GameObject[halfBeats.Length];
        for(int i = 0; i < halfBeats.Length; ++i){
            index = i + crystalOffset;
            index = index % halfBeats.Length;
            crystals[index] = this.transform.parent.GetChild(0).GetChild(0).GetChild(i).gameObject;
        }
    }
}
