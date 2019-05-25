﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractScript : MonoBehaviour
{
    public float interactDistance = 5.0f;

    private AudioSource[] getAudio;
    private GameObject intMsg;
    private Transform camera;
    private Transform player;
    private GameObject currentInteractable;

    void Start()
    {
        intMsg = GameObject.Find("InteractMessageController");
        player = GameObject.Find("Player").transform;
        camera = GameObject.Find("Player/MainCamera").transform;

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray pickRay = new Ray(camera.position, camera.forward);
      
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(pickRay, out hit, interactDistance))
            {
                if (hit.collider.tag == "Interactable")
                {
                    currentInteractable = hit.collider.gameObject;
                    if (currentInteractable.GetComponent<Animator>() != null)
                    {
                        currentInteractable.GetComponent<Animator>().SetBool("interacting", true);
                        if (currentInteractable.GetComponent<AudioSource>() != null)
                        {
                            getAudio = currentInteractable.GetComponents<AudioSource>();
                            int clipPick = Random.Range(0, getAudio.Length);
                            getAudio[clipPick].Play();


                        }
                    }
                    else
                    {
                        if (currentInteractable.GetComponent<AudioSource>() != null)
                        {
                            getAudio = currentInteractable.GetComponents<AudioSource>();
                            int clipPick = Random.Range(0, getAudio.Length);
                            getAudio[clipPick].Play();
                        }
                    }
                }
            }
        }
        else
        {
            if (Physics.Raycast(pickRay, out hit, interactDistance))
            {
                if (hit.collider.tag == "Interactable")
                {
                    intMsg.GetComponent<InteractMessage>().ShowInteractMessage("Press 'E' to interact");
                }
                else if (hit.collider.tag == "SoundObj")
                {

                }
            }
        }
    }
}