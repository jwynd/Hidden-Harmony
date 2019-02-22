using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timekeeper : MonoBehaviour
{
    [SerializeField] private int BPM = 100;

    public int BPM(){
        return BPM;
    }

    public flaot GetBeat(){
        return (60.0f / BPM);
    }

    public int SetBPM(int b){
        BPM = b;
        return BPM;
    }
    /*
    Given BPM
    USER STORY: Kiefling the Tiefling
    Stage objects define beats beginning at 1 (not 0)
    Sound object loop length defined in terms of beats (i.e. 8)
    Sound object duration of effect based on beats
    Define on sound object how many beats the effect lasts

    walking through triggers changes BPM
    */

    /*
    SerializeField BPM
    GetBPM()
    GetBeat()

    SetBPM(int b)

    */
}
