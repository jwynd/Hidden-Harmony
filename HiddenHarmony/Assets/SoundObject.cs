﻿using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SoundObject : MonoBehaviour
{
    private AudioSource aS;
    private float interactDist = 1.0f;
    private float resetTimer = 0.0f;
    public  float measureTime = 6.0f;
    private bool onStage = false;
    private float stageOffset = 0.0f;
    private string pattern = "StageObj";
    private string suffix;

    void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(this.transform.position, Vector3.down*interactDist);
    }
    // Start is called before the first frame update
    void Start(){
        aS = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate(){
        // print(resetTimer);
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
                    if(stageOffset < measureTime){
                        onStage = true;
                    } 
                    else{
                        stageOffset = 0.0f;
                    }
                }
            }
        }

        if(onStage && resetTimer > stageOffset-0.05f && resetTimer < stageOffset+0.05f){
            // print("Playing sound at time "+resetTimer);
            aS.Play();
        }


        onStage = false;
    }
}
