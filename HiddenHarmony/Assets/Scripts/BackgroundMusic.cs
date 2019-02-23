using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private int[] loopLengths;
    [SerializeField] private int[] BPMs;
    private float[] measureTimes = new float[loopLengths.Length];
    private AudioSource[] bgs;
    private float[] resetTimers = new float[measureTimes.Length];
    private Timekeeper timekeeper;
    // Start is called before the first frame update
    void Start(){
        timekeeper = GameObject.Find("Timekeeper").GetComponent<Timekeeper>();
        bgs = gameObject.GetComponents<AudioSource>();
        for(int i = 0; i < loopLengths; ++i){
            measureTimes[i] = (timekeeper.GetBeat(BPMs[i])*loopLengths[i]);
            resetTimers = 0.0f;
        }
    }

    // Update is called once per frame
    void FixedUpdate(){
        foreach(float timer in resetTimers){
            timer += time.deltaTime;
        }
        for(int i = 0; i < loopLengths.Length; ++i){
            if(resetTimers[i] > measureTimes[i]){
                resetTimers[i] = 0.0f;
                bgs[i].Play();
            }
        }
    }
}
