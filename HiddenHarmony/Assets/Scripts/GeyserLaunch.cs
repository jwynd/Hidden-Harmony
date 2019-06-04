﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeyserLaunch : MonoBehaviour
{
    public float speed = 5f;
    Vector3 movement;
    private AudioSource geyserAudio;
    private AudioSource geyserSplash;
    private GameObject player;
    private float geyserFade;
    public float geyserVolume = 0.5f;
    public float geyserSpeed;
    public float geyserAcceleration;
    private bool mute;
    // Start is called before the first frame update
    void Start(){
        player = GameObject.Find("Player");
        geyserAudio = player.transform.Find("Audio/GeyserAudio").GetComponent<AudioSource>();
        geyserSplash = player.transform.Find("Audio/GeyserSplashAudio").GetComponent<AudioSource>();
        geyserSpeed = 90f;
        geyserAcceleration = 3f;
        geyserFade = 0.05f;
        mute = true;
    }

    // Update is called once per frame
    void Update(){
        if(mute){
            player.GetComponent<PlayerMovement>().fadeToMute(geyserAudio, geyserFade);
            if(!geyserAudio.isPlaying){
                mute = false;
            }
        }
    }

    void OnTriggerEnter(Collider collide){
        if(!geyserSplash.isPlaying){
            geyserSplash.Play();
        }
        if(player.GetComponent<PlayerMovement> ().onGround){
            movement.y = collide.gameObject.GetComponent<PlayerMovement> ().canJump;
            collide.gameObject.GetComponent<CharacterController> ().Move(movement);
        }
    }

    void OnTriggerStay(Collider collide){
        TravelUp(speed, collide);
        if(!geyserAudio.isPlaying){
            geyserAudio.Play();
        }
        if(mute){
            mute = false;
        }
        player.GetComponent<PlayerMovement>().toSetVolume(geyserAudio, geyserVolume, geyserFade);
    }
    void OnTriggerExit(Collider collide){
        //TravelUp(speed * 1.5f, collide);
        if(!mute){
            mute = true;
        }
        if(!geyserSplash.isPlaying){
            geyserSplash.Play();
        }
    }

    void TravelUp(float travelSpeed , Collider collide){
        if(collide.gameObject.name == "Player"){
            collide.gameObject.GetComponent<PlayerMovement> ().yVelocity = 
            Mathf.MoveTowards(collide.gameObject.GetComponent<PlayerMovement> ().yVelocity, geyserSpeed, geyserAcceleration);
        }
    }
}
