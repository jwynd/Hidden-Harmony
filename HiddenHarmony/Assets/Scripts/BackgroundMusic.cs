using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private int[] loopLengths;
    [SerializeField] private int[] BPMs;
    private int len;
    private float[] measureTimes;
    private AudioSource[] bgs;
    private float[] resetTimers;
    private Timekeeper timekeeper;
    // Start is called before the first frame update
    void Start(){
        if(loopLengths.Length == BPMs.Length) len = loopLengths.Length;
        else throw new System.ArgumentException("Loop Lenghts have the same number of elements as BPMs");
        measureTimes = new float[len];
        resetTimers = new float[len];
        timekeeper = GameObject.Find("Timekeeper").GetComponent<Timekeeper>();
        bgs = gameObject.GetComponents<AudioSource>();
        if(bgs.Length != len) throw new System.ArgumentException("Must be as many loop lengths and BPMs as AudioSources");
        for(int i = 0; i < loopLengths.Length; ++i){
            measureTimes[i] = (timekeeper.GetBeat(BPMs[i])*loopLengths[i]);
            resetTimers[i] = 0.0f;
        }
    }

    // Update is called once per frame
    void FixedUpdate(){
        for(int i = 0; i < resetTimers.Length; ++i){
            resetTimers[i] += Time.fixedDeltaTime;
        }
        for(int i = 0; i < loopLengths.Length; ++i){
            if(resetTimers[i] > measureTimes[i]){
                resetTimers[i] = 0.0f;
                bgs[i].Play();
            }
        }
    }
}
