using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    // serialized fields allow designer to edit in inspector
    [SerializeField] private int[] loopLengths;
    public int[] BPMs; //public so it can be used by MusicChange.cs
    // len holds the length of all arrays which should be the same
    private int len;
    private float[] measureTimes;
    private AudioSource[] bgs;
    private float[] resetTimers;
    private Timekeeper timekeeper;
    // Start is called before the first frame update
    void Start(){
        // make sure that the script as been used correctly
        if(loopLengths.Length == BPMs.Length) len = loopLengths.Length;
        else throw new System.ArgumentException("Loop Lenghts have the same number of elements as BPMs");
        // initialize arrays
        measureTimes = new float[len];
        resetTimers = new float[len];
        // get the timekeeper
        timekeeper = GameObject.Find("Timekeeper").GetComponent<Timekeeper>();
        // and audiosources
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
