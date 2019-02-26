﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [HideInInspector] public bool held = false;
    public float interactDistance = 5.0f;

    private GameObject intMsg;
    private GameObject currentObject;
    private Transform camera;
    private Transform player;
    private Transform holdPosition;
    private Rigidbody rigi;
    private float dist;
    /*void OnDrawGizmos(){
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(camera.position, camera.forward*interactDistance);
    }*/

    public bool IsHeld(){
        return held;
    }
    public bool IsHeld(GameObject g){
        return held && (g == currentObject);
    }

    void Start(){
        intMsg = GameObject.Find("InteractMessageController");
        player = GameObject.Find("Player").transform;
        camera = GameObject.Find("Player/MainCamera").transform;
        holdPosition = GameObject.Find("Player/MainCamera/HoldPosition").transform;
    }

    // Update is called once per frame
    void Update(){
        if(currentObject != null) dist = Vector3.Distance(holdPosition.position, currentObject.transform.position);

        RaycastHit hit;
        Ray pickRay = new Ray(camera.position, camera.forward);
        if(!held && Input.GetKeyDown(KeyCode.E)){
            if(Physics.Raycast(pickRay, out hit, interactDistance)){
                if(hit.collider.tag == "SoundObj"){
                    //print("Raycast hit SoundObj");
                    currentObject = hit.collider.gameObject;
                    rigi = currentObject.GetComponent<Rigidbody>();
                    rigi.useGravity = false;
                    // rigi.isKinematic = true;
                    // currentObject.transform.position = holdPosition.position;
                    // print(currentObject.transform.position);
                    // currentObject.transform.parent = camera;
                    held = true;
                }
            }
            //print("!held && key press on E");
        }
        else if(!held){
            intMsg.GetComponent<InteractMessage>().HideInteractMessage();
            if(Physics.Raycast(pickRay, out hit, interactDistance)){
                if(hit.collider.tag == "SoundObj"){
                    intMsg.GetComponent<InteractMessage>().ShowInteractMessage("Press 'E' to pick up");
                }
            }
        }

        else if(held && Input.GetKeyDown(KeyCode.E)){
            //print("hend and keycode e");
            if(currentObject == null) nullObject("currentObject");
            rigi.useGravity = true;
            // rigi.isKinematic = false;
            // currentObject.transform.parent = null;
            held = false;
            rigi = null;
        }

        else if(held){
            if(currentObject == null) nullObject("currentObject");
            float step = dist*7.0f * Time.deltaTime;
            currentObject.transform.position = Vector3.MoveTowards(currentObject.transform.position, holdPosition.position, step);
            intMsg.GetComponent<InteractMessage>().HideInteractMessage();
        }

        /*if(held && currentObject != null){
            currentObject.transform.localPosition = holdPosition.localPosition;
        }*/
        //maintain the hold position above the ground
        /*Ray posRay = new Ray(holdPosition.position, Vector3.down);
        if(Physics.Raycast(posRay, out hit, 1.5f)){
            if(hit.point.y > holdPosition.position.y){
                holdPosition.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
            }
        }*/
        /*
        if(currentObject != null && held){
            // print(currentObject.transform.localPosition);
            rigi.velocity = ;
        }*/
    }

    void nullObject(string msg){
        throw new System.ArgumentException("Object "+msg+" is null");
    }
}
