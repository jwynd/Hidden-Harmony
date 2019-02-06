﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChange : MonoBehaviour
{
    public float fadeTime = 1.0f;

    private float offThreshhold = 0.05f; // threshhold at which the sound stops playing

    private bool fading = false;
    private AudioSource[] backgroundMusic;
    private GameObject[] soundObjects;
    private AudioSource oldBG;
    private AudioSource newBG;
    private int current = 0;
    // private float fadeTimer = fadeTime + 10.0f;// let fadeTimer start off
    // Start is called before the first frame update
    void Start(){
        backgroundMusic = GameObject.Find("BackgroundMusic").GetComponents<AudioSource>();
        soundObjects = GameObject.FindGameObjectsWithTag("SoundObj");
        for(int i = 1; i < backgroundMusic.Length; i++){
            backgroundMusic[i].volume = 0.0f;
        }
    }

    void Update(){
        // cross fade here
        if(fading && oldBG != null && newBG != null){
            // use linear cross fading, stretch goal uses sqrt(x)
            oldBG.volume -= Time.deltaTime/fadeTime;
            newBG.volume += Time.deltaTime/fadeTime;
            if(oldBG.volume < offThreshhold){
                newBG.volume = 1.0f;
                oldBG.volume = 0.0f;
                fading = false;
                newBG = null;
                oldBG = null;
            }
        }
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other){
        if(other.gameObject.name != "Player") return;
        if(this.name == "SubWooferTrigger" && current != 1){
            newBG = backgroundMusic[1];
            oldBG = backgroundMusic[0];
            fading = true;
            current = 1;
        } else if(this.name == "HubTrigger" && current !=0){
            newBG = backgroundMusic[0];
            oldBG = backgroundMusic[1];
            fading = true;
            current = 0;
        }
    }
}
