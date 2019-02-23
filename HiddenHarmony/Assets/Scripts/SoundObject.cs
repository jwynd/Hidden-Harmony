﻿using System; // Used for Single.TryParse
using System.Text.RegularExpressions; // used for Regex Matching
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SoundObject : MonoBehaviour
{
    [SerializeField] private int loopLength = 4;
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private float offsetRange = 0.05f;
    [HideInInspector] public bool onStage = false;

    private float beat;
    private float measureTime;
    private float vfxTimer;
    private bool vfxTimerActive = false;
    private float resetTimer = 0.0f;
    private AudioSource aS;
    private float interactDist = 1.0f;
    private float stageOffset = 0.0f;
    private string pattern = "StageObj";
    private string suffix;
    private Light light;
    private Timekeeper timekeeper;

    void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(this.transform.position, Vector3.down*interactDist);
    }
    // Start is called before the first frame update
    void Start(){
        timekeeper = GameObject.Find("Timekeeper").GetComponent<Timekeeper>();
        aS = gameObject.GetComponent<AudioSource>();
        light = gameObject.GetComponent<Light>();
        if(offsetRange <= 0.0f) throw new System.ArgumentException("Offset Range must be greater than 0");
        if(loopLength <= 1) throw new System.ArgumentException("Loop Length must be at least 1");
    }

    // Update is called once per frame
    void FixedUpdate(){
        beat = timekeeper.GetBeat();
        measureTime = beat*loopLength;
        resetTimer += Time.fixedDeltaTime;
        if(resetTimer > measureTime){
            resetTimer = 0.0f;
        }

        // determine stage by checking a ray cast, then use expression matching to determine the offset by the tag.
        RaycastHit hit;
        Ray stageRay = new Ray(this.transform.position, Vector3.down);
        if(Physics.Raycast(stageRay, out hit, interactDist)){
            Match match = Regex.Match(hit.collider.tag, pattern);
            if(match.Success){
                suffix = hit.collider.tag.Substring(8);
                if(Single.TryParse(suffix, out stageOffset)){
                    stageOffset = (stageOffset-1)*beat;
                    if(stageOffset <= measureTime && stageOffset >= 0.0f){
                        print(stageOffset);
                        print(beat);
                        print(measureTime);
                        print(resetTimer);
                        onStage = true;
                    } 
                    else{
                        onStage = false;
                    }
                }
                else{
                    onStage = false;
                }
            }
            else{
                onStage = false;
            }
        }
        else{
            onStage = false;
        }

        if(onStage && resetTimer > stageOffset-offsetRange && resetTimer < stageOffset+offsetRange){
            // print("Playing sound at time "+resetTimer);
            aS.Play();
            vfxTimerActive = true;
        }

        if(vfxTimerActive){
            print("light on");
            light.enabled = true;
        }
        else{
            light.enabled = false;
        }

        vfxTimer += Time.fixedDeltaTime;
        if(vfxTimer > duration*beat){
            vfxTimer = 0.0f;
            vfxTimerActive = false;
        }

    }
}
