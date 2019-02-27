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
    [HideInInspector] public Vector3 origin;

    private GameObject stage; // used to snap sound object to the center of the stage
    private GameObject snapPoint;
    private bool reActivateSnapPoint = false;
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
    private AudioSource[] bgs;

    void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(this.transform.position, Vector3.down*interactDist);
    }
    // Start is called before the first frame update
    void Start(){
        timekeeper = GameObject.Find("Timekeeper").GetComponent<Timekeeper>();
        if(timekeeper == null) throw new System.ArgumentException("Timekeeper null");
        aS = gameObject.GetComponent<AudioSource>();
        light = gameObject.GetComponent<Light>();
        if(offsetRange <= 0.0f) throw new System.ArgumentException("Offset Range must be greater than 0");
        if(loopLength <= 1) throw new System.ArgumentException("Loop Length must be at least 1");
        origin = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate(){
        if(timekeeper == null) throw new System.ArgumentException("Timekeeper null");
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

        if(onStage){
            stage = hit.transform.gameObject;
            if(stage.transform.childCount > 0 && 
                !GameObject.Find("Player").GetComponent<Pickup>().IsHeld(this.transform.gameObject) &&
                !reActivateSnapPoint && !stage.GetComponent<Stage>().IsOccupied()){
                snapPoint = stage.transform.GetChild(0).gameObject;
                this.transform.position = snapPoint.transform.position;
                snapPoint.transform.SetParent(null);
                reActivateSnapPoint = true;
            }
        }

        if(reActivateSnapPoint && !onStage){
            snapPoint.transform.SetParent(stage.transform);
            snapPoint.transform.SetAsFirstSibling();
            reActivateSnapPoint = false;
        }

        if(onStage && resetTimer > stageOffset-offsetRange && resetTimer < stageOffset+offsetRange){
            // print("Playing sound at time "+resetTimer);
            aS.Play();
            vfxTimerActive = true;
        }

        if(vfxTimerActive){
            // print("light on");
            vfxTimer += Time.fixedDeltaTime;
            light.enabled = true;
        }
        else{
            // print("light off");
            light.enabled = false;
        }

        if(vfxTimer > duration*beat){
            vfxTimer = 0.0f;
            vfxTimerActive = false;
        }

    }
}
