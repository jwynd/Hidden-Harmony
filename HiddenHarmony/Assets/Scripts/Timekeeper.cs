using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timekeeper : MonoBehaviour
{
    private int beatsPerMinute = 100;
    private int currentHalfBeat;
    private float timer;
    private float beat;
    void Start(){
        currentHalfBeat = 0;
        timer = 0.0f;
    }
    // update currentBeat
    void Update(){
        beat = GetBeat();
        timer += Time.deltaTime;
        if(timer > beat/2.0f){
            timer = 0.0f;
            currentHalfBeat++;
        }
    }

    public float FadeOutStartTime(int beatIndex){
        int beatsToIndex = beatIndex - currentHalfBeat;
        float time = (float)(Time.time + (beat*beatsToIndex) - timer -0.01);
        return time;
    }

    public float FadeOutEndTime(int beatIndex){
        int beatsToIndex = beatIndex - currentHalfBeat;
        float time = Time.time + (beat*beatsToIndex) - timer;
        return time;
    }

    public int CurrentHalfBeat(){
        return currentHalfBeat;
    }

    public void ResetBeat(){
        currentHalfBeat = 0;
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
