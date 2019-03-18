﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timekeeper : MonoBehaviour
{
    private int beatsPerMinute = 100;
    private int currentBeat;
    private float timer;
    private float beat;
    void Start(){
        currentBeat = 0;
        timer = 0.0f;
    }
    // update currentBeat
    void Update(){
        beat = GetBeat();
        timer += Time.deltaTime;
        if(timer > beat){
            timer = 0.0f;
            currentBeat++;
        }
    }

    public int CurrentBeat(){
        return currentBeat;
    }

    public void ResetBeat(){
        currentBeat = 0;
        timer = 0.0f;
    }

    public int BPM(){
        return beatsPerMinute;
    }

    public float GetBeat(){
        return (60.0f / beatsPerMinute);
    }
    public float GetBeat(int b){
        return (60.0f / b);
    }

    public int SetBPM(int b){
        beatsPerMinute = b;
        return beatsPerMinute;
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
